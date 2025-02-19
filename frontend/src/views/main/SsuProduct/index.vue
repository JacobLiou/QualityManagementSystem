﻿<template>
  <div>
    <a-card :bordered="false" :bodyStyle="tstyle">
      <div class="table-page-search-wrapper" v-if="hasPerm('SsuProduct:page')">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item label="产品名称">
                <a-input
                  v-model="queryParam.productName"
                  allow-clear
                  placeholder="请输入产品名称"
                  @change="handleSearch"
                  @pressEnter="$refs.table.refresh(true)"
                />
              </a-form-item> </a-col
            ><a-col :md="8" :sm="24">
              <a-form-item label="产品线">
                <a-select
                  :allowClear="true"
                  style="width: 100%"
                  v-model="queryParam.productLine"
                  placeholder="请选择产品线"
                  @change="$refs.table.refresh(true)"
                >
                  <a-select-option v-for="(item, index) in productLineData" :key="index" :value="item.code">{{
                    item.name
                  }}</a-select-option>
                </a-select>
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
        <template class="table-operator" slot="operator" v-if="hasPerm('SsuProduct:add')">
          <a-button type="primary" v-if="hasPerm('SsuProduct:add')" icon="plus" @click="$refs.addForm.AEC()"
            >新增产品线</a-button
          >
        </template>
        <span slot="productLinescopedSlots" slot-scope="text">
          {{ 'product_line' | dictType(text) }}
        </span>
        <span slot="statusscopedSlots" slot-scope="text">
          {{ 'product_status' | dictType(text) }}
        </span>
        <span slot="classificationIdscopedSlots" slot-scope="text">
          {{ 'product_classification' | dictType(text) }}
        </span>
        <span slot="action" slot-scope="text, record">
          <a v-if="hasPerm('SsuProduct:edit')" @click="$refs.addForm.AEC(record, 'edit')">编辑</a>
          <a-divider type="vertical" v-if="hasPerm('SsuProduct:edit') & hasPerm('SsuProduct:copy')" />
          <a v-if="hasPerm('SsuProduct:copy')" @click="$refs.addForm.AEC(record, 'copy')">复制</a>
          <a-divider type="vertical" v-if="hasPerm('SsuProduct:edit') & hasPerm('SsuProduct:delete')" />
          <a-popconfirm
            v-if="hasPerm('SsuProduct:delete')"
            placement="topRight"
            title="确认删除？"
            @confirm="() => SsuProductDelete(record)"
          >
            <a>删除</a>
          </a-popconfirm>
        </span>
      </s-table>
      <add-form ref="addForm" @ok="handleOk" @changePersonnel="changePersonnel" />
      <CheckUserList
        :userVisible="userVisible"
        :personnelType="personnelType"
        @checkUserArray="checkUserArray"
      ></CheckUserList>
    </a-card>
  </div>
</template>
<script>
import { STable } from '@/components'
import CheckUserList from '@/components/CheckUserList/CheckUserList.vue'
import { SsuProductPage, SsuProductDelete } from '@/api/modular/main/SsuProductManage'
import addForm from './addForm.vue'

export default {
  components: {
    STable,
    addForm,
    CheckUserList,
  },
  data() {
    return {
      queryParam: { productName: '' },
      columns: [
        {
          title: '产品名称',
          align: 'center',
          sorter: true,
          dataIndex: 'productName',
        },           
        {
          title: '状态',
          align: 'center',
          sorter: true,
          dataIndex: 'status',
          scopedSlots: { customRender: 'statusscopedSlots' },
        },
        {
          title: '产品分类',
          align: 'center',
          sorter: true,
          dataIndex: 'classificationId',
          scopedSlots: { customRender: 'classificationIdscopedSlots' },
        },
        {
          title: '产品负责人',
          align: 'center',
          sorter: true,
          dataIndex: 'directorName',
        },
      ],
      tstyle: { 'padding-bottom': '0px', 'margin-bottom': '10px' },
      // 加载数据方法 必须为 Promise 对象
      loadData: (parameter) => {
        return SsuProductPage(Object.assign(parameter, this.queryParam)).then((res) => {
          return res.data
        })
      },
      productLineData: [],
      selectedRowKeys: [],
      selectedRows: [],
      userVisible: false,
      personnelType: '',
    }
  },
  created() {
    if (this.hasPerm('SsuProduct:edit') || this.hasPerm('SsuProduct:delete')) {
      this.columns.push({
        title: '操作',
        width: '150px',
        dataIndex: 'action',
        scopedSlots: { customRender: 'action' },
        align: 'center',
      })
    }
    const productLineOption = this.$options
    this.productLineData = productLineOption.filters['dictData']('product_line')
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
    SsuProductDelete(record) {
      SsuProductDelete(record).then((res) => {
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
