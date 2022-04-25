<template>
  <div>
    <a-card :bordered="false" :bodyStyle="tstyle">

      <div class="table-page-search-wrapper" v-if="hasPerm('SsuProduct:page')">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item label="产品名称">
                <a-input v-model="queryParam.productName" allow-clear placeholder="请输入产品名称"/>
              </a-form-item>
            </a-col><a-col :md="8" :sm="24">
              <a-form-item label="产品线">
                <a-select :allowClear="true" style="width: 100%" v-model="queryParam.productLine" placeholder="请选择产品线">
                  <a-select-option v-for="(item,index) in productLineData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
                </a-select>
              </a-form-item>
            </a-col><a-col :md="8" :sm="24">
              <span class="table-page-search-submitButtons">
                <a-button type="primary" @click="$refs.table.refresh(true)">查询</a-button>
                <a-button style="margin-left: 8px" @click="() => queryParam = {}">重置</a-button>
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
        :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }">
        <template class="table-operator" slot="operator" v-if="hasPerm('SsuProduct:add')" >
          <a-button type="primary" v-if="hasPerm('SsuProduct:add')" icon="plus" @click="$refs.addForm.add()">新增产品</a-button>
        </template>
        <span slot="productLinescopedSlots" slot-scope="text">
          {{ '' | dictType(text) }}
        </span>
        <span slot="statusscopedSlots" slot-scope="text">
          {{ '' | dictType(text) }}
        </span>
        <span slot="classificationIdscopedSlots" slot-scope="text">
          {{ '' | dictType(text) }}
        </span>
        <span slot="action" slot-scope="text, record">
          <a v-if="hasPerm('SsuProduct:edit')" @click="$refs.editForm.edit(record)">编辑</a>
          <a-divider type="vertical" v-if="hasPerm('SsuProduct:edit') & hasPerm('SsuProduct:delete')"/>
          <a-popconfirm v-if="hasPerm('SsuProduct:delete')" placement="topRight" title="确认删除？" @confirm="() => SsuProductDelete(record)">
            <a>删除</a>
          </a-popconfirm>
        </span>
      </s-table>
      <add-form ref="addForm" @ok="handleOk" />
      <edit-form ref="editForm" @ok="handleOk" />
    </a-card>
  </div>
</template>
<script>
  import { STable } from '@/components'
  import { SsuProductPage, SsuProductDelete } from '@/api/modular/main/SsuProductManage'
  import addForm from './addForm.vue'
  import editForm from './editForm.vue'
  export default {
    components: {
      STable,
      addForm,
      editForm
    },
    data () {
      return {
        queryParam: {},
        columns: [
          {
            title: '产品名称',
            align: 'center',
sorter: true,
            dataIndex: 'productName'
          },
          {
            title: '产品型号',
            align: 'center',
sorter: true,
            dataIndex: 'productType'
          },
          {
            title: '产品线',
            align: 'center',
sorter: true,
            dataIndex: 'productLine',
            scopedSlots: { customRender: 'productLinescopedSlots' }
          },
          {
            title: '所属项目',
            align: 'center',
sorter: true,
            dataIndex: 'projectId'
          },
          {
            title: '状态',
            align: 'center',
sorter: true,
            dataIndex: 'status',
            scopedSlots: { customRender: 'statusscopedSlots' }
          },
          {
            title: '产品分类',
            align: 'center',
sorter: true,
            dataIndex: 'classificationId',
            scopedSlots: { customRender: 'classificationIdscopedSlots' }
          },
          {
            title: '产品负责人',
            align: 'center',
sorter: true,
            dataIndex: 'directorId'
          }
        ],
        tstyle: { 'padding-bottom': '0px', 'margin-bottom': '10px' },
        // 加载数据方法 必须为 Promise 对象
        loadData: parameter => {
          return SsuProductPage(Object.assign(parameter, this.queryParam)).then((res) => {
            return res.data
          })
        },
        productLineData: [],
        selectedRowKeys: [],
        selectedRows: []
      }
    },
    created () {
      if (this.hasPerm('SsuProduct:edit') || this.hasPerm('SsuProduct:delete')) {
        this.columns.push({
          title: '操作',
          width: '150px',
          dataIndex: 'action',
          scopedSlots: { customRender: 'action' }
        })
      }
      const productLineOption = this.$options
      this.productLineData = productLineOption.filters['dictData']('')
    },
    methods: {
      /**
       * 查询参数组装
       */
      switchingDate () {
        const obj = JSON.parse(JSON.stringify(this.queryParam))
        return obj
      },
      SsuProductDelete (record) {
        SsuProductDelete(record).then((res) => {
          if (res.success) {
            this.$message.success('删除成功')
            this.$refs.table.refresh()
          } else {
            this.$message.error('删除失败') // + res.message
          }
        })
      },
      handleOk () {
        this.$refs.table.refresh()
      },
      onSelectChange (selectedRowKeys, selectedRows) {
        this.selectedRowKeys = selectedRowKeys
        this.selectedRows = selectedRows
      }
    }
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
