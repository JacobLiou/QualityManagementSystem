<template>
  <div>
    <a-card :bordered="false" :bodyStyle="tstyle">

      <div class="table-page-search-wrapper" v-if="hasPerm('SsuIssueExtendAttribute:page')">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item label="字段编号">
                <a-input v-model="queryParam.id" allow-clear placeholder="请输入字段编号"/>
              </a-form-item>
            </a-col><a-col :md="8" :sm="24">
              <a-form-item label="模块编号">
                <a-select :allowClear="true" style="width: 100%" v-model="queryParam.module" placeholder="请选择模块编号">
                  <a-select-option v-for="(item,index) in moduleData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
                </a-select>
              </a-form-item>
            </a-col><template v-if="advanced">
              <a-col :md="8" :sm="24">
                <a-form-item label="字段名">
                  <a-input v-model="queryParam.attibuteName" allow-clear placeholder="请输入字段名"/>
                </a-form-item>
              </a-col>
              <a-col :md="8" :sm="24">
                <a-form-item label="字段代码">
                  <a-input v-model="queryParam.attributeCode" allow-clear placeholder="请输入字段代码"/>
                </a-form-item>
              </a-col><a-col :md="8" :sm="24">
                <a-form-item label="字段值类型">
                  <a-select :allowClear="true" style="width: 100%" v-model="queryParam.valueType" placeholder="请选择字段值类型">
                    <a-select-option v-for="(item,index) in valueTypeData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
                  </a-select>
                </a-form-item>
              </a-col>            </template>

            <a-col :md="8" :sm="24" >
              <span class="table-page-search-submitButtons">
                <a-button type="primary" @click="$refs.table.refresh(true)" >查询</a-button>
                <a-button style="margin-left: 8px" @click="() => queryParam = {}">重置</a-button>
                <a @click="toggleAdvanced" style="margin-left: 8px"> {{ advanced ? '收起' : '展开' }}
                  <a-icon :type="advanced ? 'up' : 'down'"/>
                </a>
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
        <template class="table-operator" slot="operator" v-if="hasPerm('SsuIssueExtendAttribute:add')" >
          <a-button type="primary" v-if="hasPerm('SsuIssueExtendAttribute:add')" icon="plus" @click="$refs.addForm.add()">新增问题扩展属性</a-button>

          <a v-if="hasPerm('SsuIssue:donwload')" @click="templateFile">
            <a-tooltip title='模板下载' placement='top'>
              <a-icon type='download' />
              模板下载
            </a-tooltip>
          </a>

          <a-upload
            :customRequest="customRequest"
            :multiple="true"
            :showUploadList="false"
            name="file"
            v-if="hasPerm('sysUser:import')">
            <a-button icon="upload">问题导入</a-button>
          </a-upload>
        </template>
        <span slot="modulescopedSlots" slot-scope="text">
          {{ 'issue_module' | dictType(text) }}
        </span>
        <span slot="valueTypescopedSlots" slot-scope="text">
          {{ 'code_gen_net_type' | dictType(text) }}
        </span>
        <span slot="action" slot-scope="text, record">
          <a v-if="hasPerm('SsuIssueExtendAttribute:edit')" @click="$refs.editForm.edit(record)">编辑</a>
          <a-divider type="vertical" v-if="hasPerm('SsuIssueExtendAttribute:edit') & hasPerm('SsuIssueExtendAttribute:delete')"/>
          <a-popconfirm v-if="hasPerm('SsuIssueExtendAttribute:delete')" placement="topRight" title="确认删除？" @confirm="() => SsuIssueExtendAttributeDelete(record)">
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
  import {
    IssueExtAttrPage,
    IssueExtAttrDeleteStruct,
    IssueExtAttrImport,
    IssueExtAttrTemplate
  } from '@/api/modular/main/SsuIssueExtendAttributeManage'
  import addForm from './addForm.vue'
  import editForm from './editForm.vue'
  import { Downloadfile } from '@/api/modular/main/SsuIssueManage'
  export default {
    components: {
      STable,
      addForm,
      editForm
    },
    data () {
      return {
        advanced: false, // 高级搜索 展开/关闭
        queryParam: {},
        columns: [
          {
            title: '字段编号',
            align: 'center',
sorter: true,
            dataIndex: 'id'
          },
          {
            title: '模块编号',
            align: 'center',
sorter: true,
            dataIndex: 'module',
            scopedSlots: { customRender: 'modulescopedSlots' }
          },
          {
            title: '字段名',
            align: 'center',
sorter: true,
            dataIndex: 'attibuteName'
          },
          {
            title: '字段代码',
            align: 'center',
sorter: true,
            dataIndex: 'attributeCode'
          },
          {
            title: '字段值类型',
            align: 'center',
sorter: true,
            dataIndex: 'valueType',
            scopedSlots: { customRender: 'valueTypescopedSlots' }
          }
        ],
        tstyle: { 'padding-bottom': '0px', 'margin-bottom': '10px' },
        // 加载数据方法 必须为 Promise 对象
        loadData: parameter => {
          return IssueExtAttrPage(Object.assign(parameter, this.queryParam)).then((res) => {
            return res.data
          })
        },
        moduleData: [],
        valueTypeData: [],
        selectedRowKeys: [],
        selectedRows: []
      }
    },
    created () {
      if (this.hasPerm('SsuIssueExtendAttribute:edit') || this.hasPerm('SsuIssueExtendAttribute:delete')) {
        this.columns.push({
          title: '操作',
          width: '150px',
          dataIndex: 'action',
          scopedSlots: { customRender: 'action' }
        })
      }
      const moduleOption = this.$options
      this.moduleData = moduleOption.filters['dictData']('issue_module')
      const valueTypeOption = this.$options
      this.valueTypeData = valueTypeOption.filters['dictData']('code_gen_net_type')
    },
    methods: {
      customRequest(data) {
        this.fileObj = data.file

        if (this.fileObj) {
          const formData = new FormData()
          formData.append('file', this.fileObj)
          // 0：正常附件 1：问题详情富文本 2：原因分析富文本 3：解决措施富文本 4：验证情况富文本
          formData.append('attachmentType', '0')
          IssueExtAttrImport(formData).then((res) => {
            if (res.success) {
              this.$message.success('导入成功')
              this.fileObj = ''
              this.$refs.table.refresh()
            } else {
              this.$message.error('导入失败：' + res.message)
            }
          })
        }
      },
      templateFile() {
        IssueExtAttrTemplate().then((res) => {
          this.confirmLoading = false
          Downloadfile(res)
          // eslint-disable-next-line handle-callback-err
        }).catch((err) => {
          this.confirmLoading = false
          this.$message.error('下载错误：获取文件流错误' + err)
        }).finally((res) => {
          this.confirmLoading = false
        })
      },
      /**
       * 查询参数组装
       */
      switchingDate () {
        const obj = JSON.parse(JSON.stringify(this.queryParam))
        return obj
      },
      SsuIssueExtendAttributeDelete (record) {
        IssueExtAttrDeleteStruct(record).then((res) => {
          if (res.success) {
            this.$message.success('删除成功')
            this.$refs.table.refresh()
          } else {
            this.$message.error('删除失败') // + res.message
          }
        })
      },
      toggleAdvanced () {
        this.advanced = !this.advanced
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
