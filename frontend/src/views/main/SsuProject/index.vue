<template>
  <div>
    <a-card :bordered="false" :bodyStyle="tstyle">
      <div class="table-page-search-wrapper" v-if="hasPerm('SsuProject:page')">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item label="项目名称">
                <a-input
                  v-model="queryParam.projectName"
                  allow-clear
                  placeholder="请输入项目名称"
                  @change="handleSearch"
                  @pressEnter="$refs.table.refresh(true)"
                />
              </a-form-item> </a-col
            ><a-col :md="8" :sm="24">
              <span class="table-page-search-submitButtons">
                <a-button type="primary" @click="$refs.table.refresh(true)">查询</a-button>
                <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
              </span>
            </a-col>
          </a-row>
        </a-form>
      </div>
    </a-card>
    <a-card :bordered="false">
      <s-table
        ref="table"
        :columns="columns"
        :data="loadData"
        :alert="true"
        :rowKey="(record) => record.id"
        :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
      >
        <template class="table-operator" slot="operator" v-if="hasPerm('SsuProject:add')">
          <a-button type="primary" v-if="hasPerm('SsuProject:add')" icon="plus" @click="$refs.addForm.AEC()"
            >新增产品项目</a-button
          >
        </template>
        <span slot="action" slot-scope="text, record">
          <a v-if="hasPerm('SsuProject:edit')" @click="$refs.addForm.AEC(record, 'edit')">编辑</a>
          <a-divider type="vertical" v-if="hasPerm('SsuProject:edit') & hasPerm('SsuProject:copy')" />
          <a v-if="hasPerm('SsuProject:copy')" @click="$refs.addForm.AEC(record, 'copy')">复制</a>
          <a-divider type="vertical" v-if="hasPerm('SsuProject:edit') & hasPerm('SsuProject:delete')" />
          <a-popconfirm
            v-if="hasPerm('SsuProject:delete')"
            placement="topRight"
            title="确认删除？"
            @confirm="() => SsuProjectDelete(record)"
          >
            <a>删除</a>
          </a-popconfirm>
        </span>
      </s-table>
      <add-form ref="addForm" @ok="handleOk" @changePersonnel="changePersonnel" />
    </a-card>
    <CheckUserList
      :userVisible="userVisible"
      :personnelType="personnelType"
      @checkUserArray="checkUserArray"
    ></CheckUserList>
  </div>
</template>
<script>
import { STable } from '@/components'
import CheckUserList from '@/components/CheckUserList/CheckUserList.vue'
import { SsuProjectPage, SsuProjectDelete } from '@/api/modular/main/SsuProjectManage'
import addForm from './addForm.vue'
export default {
  components: {
    STable,
    addForm,
    CheckUserList,
  },
  data() {
    return {
      queryParam: {},
      columns: [
        {
          title: '项目名称',
          align: 'center',
          sorter: true,
          dataIndex: 'projectName',
        },
        {
          title: '项目负责人',
          align: 'center',
          sorter: true,
          dataIndex: 'directorName',
        },
        {
          title: '排序',
          align: 'center',
          sorter: true,
          dataIndex: 'sort',
        },
        {
          title: '项目编号',
          align: 'center',
          sorter: true,
          dataIndex: 'id',
        },
      ],
      tstyle: { 'padding-bottom': '0px', 'margin-bottom': '10px' },
      // 加载数据方法 必须为 Promise 对象
      loadData: (parameter) => {
        return SsuProjectPage(Object.assign(parameter, this.queryParam)).then((res) => {
          return res.data
        })
      },
      selectedRowKeys: [],
      selectedRows: [],
      userVisible: false,
      personnelType: '',
    }
  },
  created() {
    if (this.hasPerm('SsuProject:edit') || this.hasPerm('SsuProject:delete')) {
      this.columns.push({
        title: '操作',
        width: '150px',
        dataIndex: 'action',
        scopedSlots: { customRender: 'action' },
        align: 'center',
      })
    }
  },
  methods: {
    // 清除
    handleSearch(e) {
      if (e.isTrusted) {
        this.$refs.table.refresh(true)
      }
    },

    changePersonnel(value) {
      this.userVisible = !this.userVisible
      this.personnelType = value
    },
    // 人员选择
    checkUserArray(checkUser) {
      const perArray = checkUser.map((item) => {
        return item.name
      })
      this.$refs.addForm.form.setFieldsValue({
        directorId: Number(checkUser[0].id),
      })
      this.$refs.addForm.directorName = perArray.join()
    },
    /**
     * 查询参数组装
     */
    switchingDate() {
      const obj = JSON.parse(JSON.stringify(this.queryParam))
      return obj
    },
    SsuProjectDelete(record) {
      SsuProjectDelete(record).then((res) => {
        if (res.success) {
          this.$message.success('删除成功')
          this.$refs.table.refresh()
        } else {
          this.$message.error('删除失败,'  + res.message)
        }
      })
    },
    handleOk() {
      this.$refs.table.refresh()
    },
    onSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
    },
  },
}
</script>
<style lang="less">
.table-operator {
  margin-bottom: 18px;
}
button {
  margin-right: 8px;
}
</style>
