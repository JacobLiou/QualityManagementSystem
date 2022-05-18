<template>
  <div>
    <a-card :bordered="false" :bodyStyle="tstyle">

      <div class="table-page-search-wrapper" v-if="hasPerm('SsuIssue:page')">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="12">
              <a-form-item label="项目编号">
                <a-select :allowClear="true" style="width: 100%" v-model="queryParam.projectId" placeholder="请输入项目编号">
                  <a-select-option v-for="(item,index) in projectData" :key="index" :value="item.id">{{ item.projectName }}</a-select-option>
                </a-select>
              </a-form-item>
            </a-col>
            <template v-if="advanced">
              <a-col :md="8" :sm="12">
                <a-form-item label="问题模块">
                  <a-select :allowClear="true" style="width: 100%" v-model="queryParam.module" placeholder="请选择问题模块">
                    <a-select-option v-for="(item,index) in moduleData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
                  </a-select>
                </a-form-item>
              </a-col>
              <a-col :md="8" :sm="24">
                <a-form-item label="问题性质">
                  <a-select :allowClear="true" style="width: 100%" v-model="queryParam.consequence" placeholder="请选择问题性质">
                    <a-select-option v-for="(item,index) in consequenceData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
                  </a-select>
                </a-form-item>
              </a-col>
              <a-col :md="8" :sm="24">
                <a-form-item label="问题状态">
                  <a-select :allowClear="true" style="width: 100%" v-model="queryParam.status" placeholder="请选择问题状态">
                    <a-select-option v-for="(item,index) in statusData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
                  </a-select>
                </a-form-item>
              </a-col>
            </template>
            <a-col :md="8" :sm="24">
              <a-form-item label="关键词">
                <a-input v-model="queryParam.title" allow-clear placeholder="请输入问题关键词"/>
              </a-form-item>
            </a-col>

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
        :useFixedHeader="true"
        :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
        :rowKey="(record) => record.id">
        <template class="table-operator" slot="operator" v-if="hasPerm('SsuIssue:add')" >
          <a-button type="primary" v-if="hasPerm('SsuIssue:add')" icon="plus" @click="$refs.addForm.add()">新增问题记录</a-button>
          <a :class='{"active": queryBy == 0}' @click="query(0)">所有</a>
          <a-divider type="vertical"/>
          <a :class='{"active": queryBy == 1}' @click="query(1)">由我创建</a>
          <a-divider type="vertical"/>
          <a :class='{"active": queryBy == 2}' @click="query(2)">指派给我</a>
          <a-divider type="vertical"/>
          <a :class='{"active": queryBy == 3}' @click="query(3)">由我解决</a>
          <a-divider type="vertical"/>
          <a :class='{"active": queryBy == 4}' @click="query(4)">待验证</a>
          <a-divider type="vertical"/>
          <a :class='{"active": queryBy == 5}' @click="query(5)">未解决</a>
          <a-divider type="vertical"/>
          <a :class='{"active": queryBy == 6}' @click="query(6)">已关闭</a>
          <a-divider type="vertical"/>
          <a :class='{"active": queryBy == 7}' @click="query(7)">已挂起</a>
          <!--          <a-divider type="vertical"/>-->
          <!--          <a :class='{"active": queryBy == 8}' @click="query(8)">抄送给我</a>-->

          <a-button type="primary" v-if="hasPerm('SsuIssue:export')" icon="export" @click="exportData">问题导出</a-button>

          <a v-if="hasPerm('SsuIssue:donwload')" @click="templateFile">
            <a-tooltip title='模板下载' placement='top'>
              <a-icon type='download' />
              模板下载
            </a-tooltip>
          </a>

          <a v-if="hasPerm('SsuIssue:donwload')" @click="downloadFile">
            <a-tooltip title='附件下载' placement='top'>
              <a-icon type='download' />
              附件下载
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
          {{ 'issue_status' | dictType(text) }}
        </span>
        <span slot="action" slot-scope="text, record">
          <a v-if="hasPerm('SsuIssue:copy')" @click="$refs.addForm.copy(record)">
            <a-tooltip title='复制' placement='top'>
              <a-icon type='copy' />
            </a-tooltip>
          </a>
          <a-divider type="vertical" v-if="hasPerm('SsuIssue:edit') & hasPerm('SsuIssue:copy')"/>
          <a v-if="hasPerm('SsuIssue:dispatch')" @click="$refs.dispatchForm.edit(record)">
            <a-tooltip title='分发' placement='top'>
              <a-icon type='swap' />
            </a-tooltip>
          </a>
          <a-divider type="vertical" v-if="hasPerm('SsuIssue:edit') & hasPerm('SsuIssue:delete')"/>
          <a v-if="hasPerm('SsuIssue:execute')" @click="$refs.executeForm.edit(record)">
            <a-tooltip title='解决' placement='top'>
              <a-icon type='check' />
            </a-tooltip>
          </a>
          <a-divider type="vertical" v-if="hasPerm('SsuIssue:edit') & hasPerm('SsuIssue:delete')"/>
          <a v-if="hasPerm('SsuIssue:validate')" @click="$refs.validateForm.edit(record)">
            <a-tooltip title='验证' placement='top'>
              <a-icon type='meh' />
            </a-tooltip>
          </a>
          <a-divider type="vertical" v-if="hasPerm('SsuIssue:edit') & hasPerm('SsuIssue:delete')"/>
          <a v-if="hasPerm('SsuIssue:dispatch')" @click="$refs.redispatchForm.edit(record)">
            <a-tooltip title='转交' placement='top'>
              <a-icon type='rollback' />
            </a-tooltip>
          </a>
          <a-divider type="vertical" v-if="hasPerm('SsuIssue:edit') & hasPerm('SsuIssue:delete')"/>
          <a v-if="hasPerm('SsuIssue:hangup')" @click="$refs.hangupForm.edit(record)">
            <a-tooltip title='挂起' placement='top'>
              <a-icon type='question' />
            </a-tooltip>
          </a>
          <a-divider type="vertical" v-if="hasPerm('SsuIssue:edit') & hasPerm('SsuIssue:delete')"/>
          <a v-if="hasPerm('SsuIssue:hangup')" @click="$refs.recheckForm.edit(record)">
            <a-tooltip title='复核' placement='top'>
              <a-icon type='user' />
            </a-tooltip>
          </a>
          <a-divider type="vertical" v-if="hasPerm('SsuIssue:edit') & hasPerm('SsuIssue:delete')"/>
          <a v-if="hasPerm('SsuIssue:hangup')" @click="reOpen(record)">
            <a-tooltip title='重开启' placement='top'>
              <a-icon type='unlock' />
            </a-tooltip>
          </a>
          <a-divider type="vertical" v-if="hasPerm('SsuIssue:edit') & hasPerm('SsuIssue:delete')"/>
          <a v-if="hasPerm('SsuIssue:edit')" @click="$refs.editForm.edit(record)">
            <a-tooltip title='编辑' placement='top'>
              <a-icon type='edit' />
            </a-tooltip>
          </a>
          <a-divider type="vertical" v-if="hasPerm('SsuIssue:edit') & hasPerm('SsuIssue:delete')"/>
          <a-popconfirm v-if="hasPerm('SsuIssue:delete')" placement="topRight" title="确认删除？" @confirm="() => Delete(record)">
            <a-tooltip title='删除' placement='top'>
              <a-icon type='delete' />
            </a-tooltip>
          </a-popconfirm>
        </span>
      </s-table>
      <add-form ref="addForm" @ok="handleOk" />
      <edit-form ref="editForm" @ok="handleOk" />

      <dispatch-form ref="dispatchForm" @ok="handleOk" />
      <execute-form ref="executeForm" @ok="handleOk" />
      <validate-form ref="validateForm" @ok="handleOk" />
      <redispatch-form ref="redispatchForm" @ok="handleOk" />
      <hangup-form ref="hangupForm" @ok="handleOk" />

      <recheck-form ref='recheckForm' @ok='handleOk'/>
    </a-card>
  </div>
</template>
<script>
import { STable } from '@/components'
import {
  IssuePage,
  IssueDelete,
  IssueExport,
  IssueTemplate,
  IssueImport,
  IssueReOpen,
  Downloadfile
} from '@/api/modular/main/SsuIssueManage'
import executeForm from './executeForm.vue'
import validateForm from './validateForm.vue'
import hangupForm from './hangupForm.vue'
import dispatchForm from './dispatchForm.vue'
import redispatchForm from './redispatchForm.vue'

import recheckForm from './recheckForm.vue'

import addForm from './addForm.vue'
import editForm from './editForm.vue'

import {
  SsuProjectList
} from '@/api/modular/main/SsuProjectManage'
import { sysFileInfoDownload } from '@/api/modular/system/fileManage'
export default {
  components: {
    STable,
    addForm,
    editForm,
    executeForm,
    dispatchForm,
    validateForm,
    hangupForm,
    redispatchForm,
    recheckForm
  },
  data () {
    return {
      queryBy: 0,
      advanced: false, // 高级搜索 展开/关闭
      queryParam: {},
      columns: [
        {
          title: '序号',
          align: 'center',
          sorter: true,
          dataIndex: 'id'
          // visible: false
        },
        {
          title: '标题',
          align: 'center',
          sorter: true,
          dataIndex: 'title'
        },
        {
          title: '项目名',
          align: 'center',
          sorter: true,
          dataIndex: 'projectName'
        },
        {
          title: '产品名',
          align: 'center',
          sorter: true,
          dataIndex: 'productName'
        },
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
          dataIndex: 'creatorName'
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
          dataIndex: 'discoverName'
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
          dataIndex: 'dispatcherName'
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
          dataIndex: 'copyToName'
        },
        {
          title: '解决人',
          align: 'center',
          sorter: true,
          dataIndex: 'executorName'
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
          dataIndex: 'verifierName'
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
        return IssuePage(Object.assign(parameter, this.queryParam)).then((res) => {
          return res.data
        })
      },
      moduleData: [],
      consequenceData: [],
      statusData: [],
      selectedRowKeys: [],
      selectedRows: [],
      defaultColumns: [],
      projectData: [],
      fileObj: ''
    }
  },
  created () {
    if (this.hasPerm('SsuIssue:edit') || this.hasPerm('SsuIssue:delete')) {
      // 测试
      this.columns.push({
        title: '操作',
        width: '360px',
        dataIndex: 'action',
        scopedSlots: { customRender: 'action' }
      })
    }

    SsuProjectList().then((res) => {
      if (res.success) {
        this.projectData = res.data
      } else {
        this.$message.error('项目列表读取失败')
      }
    }).finally((res) => {
      this.confirmLoading = false
    })

    const moduleOption = this.$options
    this.moduleData = moduleOption.filters['dictData']('issue_module')
    const consequenceOption = this.$options
    this.consequenceData = consequenceOption.filters['dictData']('issue_consequence')
    const statusOption = this.$options
    this.statusData = statusOption.filters['dictData']('issue_status')
  },
  methods: {
    customRequest(data) {
      this.fileObj = data.file

      if (this.fileObj) {
        const formData = new FormData()
        formData.append('file', this.fileObj)
        IssueImport(formData).then((res) => {
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
    query(index) {
      this.queryParam.QueryCondition = index
      this.queryBy = index
      this.$refs.table.refresh(true)
    },
    reOpen(record) {
      IssueReOpen(record).then((res) => {
        if (res.success) {
          this.$message.success('开启成功')
          this.$refs.table.refresh()
        } else {
          this.$message.error('开启失败!' + res.message) // + res.message
        }
      })
    },
    exportData() {
      IssueExport(this.queryParam).then((res) => {
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
    templateFile() {
      IssueTemplate().then((res) => {
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
    downloadFile() {
      // this.model.issueId = 286390745276485
      // this.model.attachmentId = 285680457277509

      var model = {
        Id: 277503259144261
      }

      sysFileInfoDownload(model).then((res) => {
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
    Delete (record) {
      IssueDelete(record).then((res) => {
        if (res.success) {
          this.$message.success('删除成功')
          this.$refs.table.refresh()
        } else {
          this.$message.error('删除失败'+ res.message) // + res.message
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
.s-table-tool-left {
  margin-bottom: 18px;
}
button {
  margin-right: 8px;
}

.s-table-tool-left > a {
  padding: 5px;
}

.s-table-tool-left > a.active {
  background: #dddddd;
  border-radius: 5px;
}

.ant-btn-primary {
  margin: 0px 10px;
}

</style>
