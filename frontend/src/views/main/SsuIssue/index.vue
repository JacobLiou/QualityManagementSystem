<template>
  <div>
    <a-card :bordered="false" :bodyStyle="tstyle">

      <div class="table-page-search-wrapper" v-if="hasPerm('SsuIssue:page')">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item label="问题简述">
                <a-input v-model="queryParam.title" allow-clear placeholder="请输入问题简述"/>
              </a-form-item>
            </a-col><a-col :md="8" :sm="24">
              <a-form-item label="项目编号">
                <a-input-number v-model="queryParam.projectId" style="width: 100%" allow-clear placeholder="请输入项目编号"/>
              </a-form-item>
            </a-col><template v-if="advanced"><a-col :md="8" :sm="24">
                <a-form-item label="问题模块">
                  <a-select :allowClear="true" style="width: 100%" v-model="queryParam.module" placeholder="请选择问题模块">
                    <a-select-option v-for="(item,index) in moduleData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
                  </a-select>
                </a-form-item>
              </a-col><a-col :md="8" :sm="24">
                <a-form-item label="问题性质">
                  <a-select :allowClear="true" style="width: 100%" v-model="queryParam.consequence" placeholder="请选择问题性质">
                    <a-select-option v-for="(item,index) in consequenceData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
                  </a-select>
                </a-form-item>
              </a-col><a-col :md="8" :sm="24">
                <a-form-item label="问题状态">
                  <a-select :allowClear="true" style="width: 100%" v-model="queryParam.status" placeholder="请选择问题状态">
                    <a-select-option v-for="(item,index) in statusData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
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
        <template class="table-operator" slot="operator" v-if="hasPerm('SsuIssue:add')" >
          <a-button type="primary" v-if="hasPerm('SsuIssue:add')" icon="plus" @click="$refs.addForm.add()">新增问题记录</a-button>
        </template>
        <span slot="modulescopedSlots" slot-scope="text">
          {{ 'issue_module' | dictType(text) }}
        </span>
        <span slot="consequencescopedSlots" slot-scope="text">
          {{ 'issue_consequence' | dictType(text) }}
        </span>
        <span slot="issueClassificationscopedSlots" slot-scope="text">
          {{ 'issue_classification' | dictType(text) }}
        </span>
        <span slot="sourcescopedSlots" slot-scope="text">
          {{ 'issue_source' | dictType(text) }}
        </span>
        <span slot="statusscopedSlots" slot-scope="text">
          {{ 'isssue_status' | dictType(text) }}
        </span>
        <span slot="action" slot-scope="text, record">
          <a v-if="hasPerm('SsuIssue:edit')" @click="$refs.editForm.edit(record)">编辑</a>
          <a-divider type="vertical" v-if="hasPerm('SsuIssue:edit') & hasPerm('SsuIssue:delete')"/>
          <a-popconfirm v-if="hasPerm('SsuIssue:delete')" placement="topRight" title="确认删除？" @confirm="() => SsuIssueDelete(record)">
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
  import { SsuIssuePage, SsuIssueDelete } from '@/api/modular/main/SsuIssueManage'
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
        advanced: false, // 高级搜索 展开/关闭
        queryParam: {},
        columns: [
          {
            title: '问题模块',
            align: 'center',
sorter: true,
            dataIndex: 'module',
            scopedSlots: { customRender: 'modulescopedSlots' }
          },
          {
            title: '问题性质',
            align: 'center',
sorter: true,
            dataIndex: 'consequence',
            scopedSlots: { customRender: 'consequencescopedSlots' }
          },
          {
            title: '问题分类',
            align: 'center',
sorter: true,
            dataIndex: 'issueClassification',
            scopedSlots: { customRender: 'issueClassificationscopedSlots' }
          },
          {
            title: '问题来源',
            align: 'center',
sorter: true,
            dataIndex: 'source',
            scopedSlots: { customRender: 'sourcescopedSlots' }
          },
          {
            title: '问题状态',
            align: 'center',
sorter: true,
            dataIndex: 'status',
            scopedSlots: { customRender: 'statusscopedSlots' }
          },
          {
            title: '提出人',
            align: 'center',
sorter: true,
            dataIndex: 'creatorId'
          },
          {
            title: '提出日期',
            align: 'center',
sorter: true,
            dataIndex: 'createTime'
          },
          {
            title: '关闭日期',
            align: 'center',
sorter: true,
            dataIndex: 'closeTime'
          },
          {
            title: '发现人',
            align: 'center',
sorter: true,
            dataIndex: 'discover'
          },
          {
            title: '发现日期',
            align: 'center',
sorter: true,
            dataIndex: 'discoverTime'
          },
          {
            title: '分发人',
            align: 'center',
sorter: true,
            dataIndex: 'dispatcher'
          },
          {
            title: '分发日期',
            align: 'center',
sorter: true,
            dataIndex: 'dispatchTime'
          },
          {
            title: '预计完成日期',
            align: 'center',
sorter: true,
            dataIndex: 'forecastSolveTime'
          },
          {
            title: '被抄送人',
            align: 'center',
sorter: true,
            dataIndex: 'cC'
          },
          {
            title: '解决人',
            align: 'center',
sorter: true,
            dataIndex: 'executor'
          },
          {
            title: '解决日期',
            align: 'center',
sorter: true,
            dataIndex: 'solveTime'
          },
          {
            title: '验证人',
            align: 'center',
sorter: true,
            dataIndex: 'verifier'
          },
          {
            title: '验证地点',
            align: 'center',
sorter: true,
            dataIndex: 'verifierPlace'
          },
          {
            title: '验证日期',
            align: 'center',
sorter: true,
            dataIndex: 'validateTime'
          }
        ],
        tstyle: { 'padding-bottom': '0px', 'margin-bottom': '10px' },
        // 加载数据方法 必须为 Promise 对象
        loadData: parameter => {
          return SsuIssuePage(Object.assign(parameter, this.queryParam)).then((res) => {
            return res.data
          })
        },
        moduleData: [],
        consequenceData: [],
        statusData: [],
        selectedRowKeys: [],
        selectedRows: []
      }
    },
    created () {
      if (this.hasPerm('SsuIssue:edit') || this.hasPerm('SsuIssue:delete')) {
        this.columns.push({
          title: '操作',
          width: '150px',
          dataIndex: 'action',
          scopedSlots: { customRender: 'action' }
        })
      }
      const moduleOption = this.$options
      this.moduleData = moduleOption.filters['dictData']('issue_module')
      const consequenceOption = this.$options
      this.consequenceData = consequenceOption.filters['dictData']('issue_consequence')
      const statusOption = this.$options
      this.statusData = statusOption.filters['dictData']('isssue_status')
    },
    methods: {
      /**
       * 查询参数组装
       */
      switchingDate () {
        const obj = JSON.parse(JSON.stringify(this.queryParam))
        return obj
      },
      SsuIssueDelete (record) {
        SsuIssueDelete(record).then((res) => {
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
