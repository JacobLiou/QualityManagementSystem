﻿<template>
  <div>
    <a-card :bordered="false" :bodyStyle="tstyle">
      <div class="table-page-search-wrapper" v-if="hasPerm('SsuGroup:page')">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item label="人员组名称">
                <a-input
                  v-model="queryParam.groupName"
                  allow-clear
                  placeholder="请输入人员组名称"
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
        <template class="table-operator" slot="operator" v-if="hasPerm('SsuGroup:add')">
          <a-button type="primary" v-if="hasPerm('SsuGroup:add')" icon="plus" @click="$refs.addForm.AEC()"
            >新增人员组</a-button
          >
        </template>
        <span slot="action" slot-scope="text, record">
          <a v-if="hasPerm('SsuGroup:edit')" @click="$refs.addForm.AEC(record, 'edit')">编辑</a>
          <a-divider type="vertical" v-if="hasPerm('SsuGroup:edit') & hasPerm('SsuGroup:copy')" />
          <a v-if="hasPerm('SsuGroup:copy')" @click="$refs.addForm.AEC(record, 'copy')">复制</a>
          <a-divider type="vertical" v-if="hasPerm('SsuGroup:edit') & hasPerm('SsuGroup:delete')" />
          <a-popconfirm
            v-if="hasPerm('SsuGroup:delete')"
            placement="topRight"
            title="确认删除？"
            @confirm="() => SsuGroupDelete(record)"
          >
            <a>删除</a>
          </a-popconfirm>
        </span>
      </s-table>
      <add-form ref="addForm" @ok="handleOk" />
    </a-card>
  </div>
</template>
<script>
import { STable } from '@/components'
import { SsuGroupPage, SsuGroupDelete } from '@/api/modular/main/SsuGroupManage'
import addForm from './addForm.vue'
export default {
  components: {
    STable,
    addForm,
  },
  data() {
    return {
      queryParam: {},
      columns: [
        {
          title: '人员组名称',
          align: 'center',
          sorter: true,
          dataIndex: 'groupName',
        },
        {
          title: '排序',
          align: 'center',
          sorter: true,
          dataIndex: 'sort',
        },
      ],
      tstyle: { 'padding-bottom': '0px', 'margin-bottom': '10px' },
      // 加载数据方法 必须为 Promise 对象
      loadData: (parameter) => {
        return SsuGroupPage(Object.assign(parameter, this.queryParam)).then((res) => {
          return res.data
        })
      },
      selectedRowKeys: [],
      selectedRows: [],
    }
  },
  created() {
    if (this.hasPerm('SsuGroup:edit') || this.hasPerm('SsuGroup:delete')) {
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
    /**
     * 查询参数组装
     */
    switchingDate() {
      const obj = JSON.parse(JSON.stringify(this.queryParam))
      return obj
    },
    SsuGroupDelete(record) {
      SsuGroupDelete(record).then((res) => {
        if (res.success) {
          this.$message.success('删除成功')
          this.$refs.table.refresh()
        } else {
          this.$message.error('删除失败') // + res.message
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
