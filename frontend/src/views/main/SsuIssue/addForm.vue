<script src='../../../api/modular/main/SsuIssueManage.js'></script>
<template>
  <a-modal
    title="新增问题记录"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item label="问题简述" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入问题简述" v-decorator="['title', {rules: [{required: true, message: '请输入问题简述！'}]}]" />
        </a-form-item>
        <a-form-item label="问题详情" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入问题详情" v-decorator="['description', {rules: [{required: true, message: '请输入问题详情！'}]}]" />
        </a-form-item>
        <a-form-item label="项目编号" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
            <a-select :allowClear="true" placeholder="请选择项目" style="width: 100%" v-decorator="['projectId', {rules: [{ required: true, message: '请选择项目！' }]}]">
              <a-select-option v-for="(item,index) in projectData" :key="index" :value="item.id">{{ item.projectName }}</a-select-option>
            </a-select>
        </a-form-item>
        <a-form-item label="产品编号" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-select :allowClear="true" placeholder="请选择产品" style="width: 100%" v-decorator="['productId', {rules: [{ required: true, message: '请选择产品！' }]}]">
            <a-select-option v-for="(item,index) in productData" :key="index" :value="item.id">{{ item.productName }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="问题模块" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select style="width: 100%" placeholder="请选择问题模块" v-decorator="['module', {rules: [{ required: true, message: '请选择问题模块！' }]}]">
            <a-select-option v-for="(item,index) in moduleData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="问题性质" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select style="width: 100%" placeholder="请选择问题性质" v-decorator="['consequence', {rules: [{ required: true, message: '请选择问题性质！' }]}]">
            <a-select-option v-for="(item,index) in consequenceData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="问题分类" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select style="width: 100%" placeholder="请选择问题分类" v-decorator="['issueClassification', {rules: [{ required: true, message: '请选择问题分类！' }]}]">
            <a-select-option v-for="(item,index) in issueClassificationData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="分发人" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
<!--          <a-input-number placeholder="请输入分发人" style="width: 100%" v-decorator="['dispatcher', {rules: [{required: true, message: '请输入指定的分发人！'}]}]" />-->

          <a-tree-select
            v-decorator="['dispatcher', {rules: [{ required: true, message: '请选择指定的分发人' }]}]"
            style="width: 100%"
            :dropdownStyle="{ maxHeight: '300px', overflow: 'auto' }"
            :treeData="orgTree"
            placeholder="请选择人员"
            treeDefaultExpandAll
          >
            <span slot="title" slot-scope="{ id }">{{ id }}</span>
          </a-tree-select>
        </a-form-item>

<!--        <a-form-item label="问题状态" :labelCol="labelCol" :wrapperCol="wrapperCol">-->
<!--          <a-select style="width: 100%" placeholder="请选择问题状态" v-decorator="['status', {rules: [{ required: true, message: '请选择问题状态！' }]}]">-->
<!--            <a-select-option v-for="(item,index) in statusData" :key="index" :value="item.code">{{ item.name }}</a-select-option>-->
<!--          </a-select>-->
<!--        </a-form-item>-->
<!--        <a-form-item label="提出人" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>-->
<!--          <a-input-number placeholder="请输入提出人" style="width: 100%" v-decorator="['creatorId']" />-->
<!--        </a-form-item>-->
<!--        <a-form-item label="提出日期" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>-->
<!--          <a-date-picker style="width: 100%" placeholder="请选择提出日期" v-decorator="['createTime']" @change="onChangecreateTime"/>-->
<!--        </a-form-item>-->
<!--        <a-form-item label="关闭日期" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>-->
<!--          <a-date-picker style="width: 100%" placeholder="请选择关闭日期" v-decorator="['closeTime']" @change="onChangecloseTime"/>-->
<!--        </a-form-item>-->
        <a-form-item label="问题来源" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select style="width: 100%" placeholder="请选择问题来源" v-decorator="['source', {rules: [{ required: true, message: '请选择问题来源！' }]}]">
            <a-select-option v-for="(item,index) in sourceData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="发现人" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input-number placeholder="请输入发现人" style="width: 100%" v-decorator="['discover']" />
        </a-form-item>
        <a-form-item label="发现日期" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-date-picker style="width: 100%" placeholder="请选择发现日期" v-decorator="['discoverTime']" @change="onChangediscoverTime"/>
        </a-form-item>

<!--        <a-form-item label="分发日期" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>-->
<!--          <a-date-picker style="width: 100%" placeholder="请选择分发日期" v-decorator="['dispatchTime']" @change="onChangedispatchTime"/>-->
<!--        </a-form-item>-->
<!--        <a-form-item label="预计完成日期" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>-->
<!--          <a-date-picker style="width: 100%" placeholder="请选择预计完成日期" v-decorator="['forecastSolveTime']" @change="onChangeforecastSolveTime"/>-->
<!--        </a-form-item>-->
        <a-form-item label="被抄送人" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input-number placeholder="请输入被抄送人" style="width: 100%" v-decorator="['cC']" />
        </a-form-item>
<!--        <a-form-item label="解决人" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>-->
<!--          <a-input-number placeholder="请输入解决人" style="width: 100%" v-decorator="['executor']" />-->
<!--        </a-form-item>-->
<!--        <a-form-item label="解决日期" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>-->
<!--          <a-date-picker style="width: 100%" placeholder="请选择解决日期" v-decorator="['solveTime']" @change="onChangesolveTime"/>-->
<!--        </a-form-item>-->
<!--        <a-form-item label="验证人" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>-->
<!--          <a-input-number placeholder="请输入验证人" style="width: 100%" v-decorator="['verifier']" />-->
<!--        </a-form-item>-->
<!--        <a-form-item label="验证地点" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>-->
<!--          <a-input placeholder="请输入验证地点" v-decorator="['verifierPlace']" />-->
<!--        </a-form-item>-->
<!--        <a-form-item label="验证日期" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>-->
<!--          <a-date-picker style="width: 100%" placeholder="请选择验证日期" v-decorator="['validateTime']" @change="onChangevalidateTime"/>-->
<!--        </a-form-item>-->

        <a-form-item label="附件上传" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-upload
            :customRequest="customRequest"
            :multiple="true"
            :showUploadList="true"
            name="file"
            v-if="hasPerm('sysUser:import')">
            <a-button icon="upload">附件上传</a-button>
          </a-upload>
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import {
  IssueAdd, IssueAttachmentSaveId
} from '@/api/modular/main/SsuIssueManage'

  import moment from 'moment'

  import {
    SsuProjectList
  } from '@/api/modular/main/SsuProjectManage'

  import {
    SsuProductList
  } from '@/api/modular/main/SsuProductManage'

  import { sysUserOrgTree } from '@/api/modular/system/userManage'
  import { sysFileInfoUpload } from '@/api/modular/system/fileManage'

  export default {
    data () {
      return {
        labelCol: {
          xs: { span: 24 },
          sm: { span: 5 }
        },
        wrapperCol: {
          xs: { span: 24 },
          sm: { span: 15 }
        },
        moduleData: [],
        consequenceData: [],
        issueClassificationData: [],
        sourceData: [],
        statusData: [],
        createTimeDateString: '',
        closeTimeDateString: '',
        discoverTimeDateString: '',
        dispatchTimeDateString: '',
        forecastSolveTimeDateString: '',
        solveTimeDateString: '',
        validateTimeDateString: '',
        visible: false,
        confirmLoading: false,
        form: this.$form.createForm(this),
        projectData: [],
        productData: [],
        orgTree: [],
        fileObj: ''
      }
    },
    created () {
      SsuProjectList().then((res) => {
        if (res.success) {
          this.projectData = res.data
        } else {
          this.$message.error('项目列表读取失败')
        }
      }).finally((res) => {
        this.confirmLoading = false
      })

      SsuProductList().then((res) => {
        if (res.success) {
          this.productData = res.data
        } else {
          this.$message.error('产品列表读取失败')
        }
      }).finally((res) => {
        this.confirmLoading = false
      })

      sysUserOrgTree().then((res) => {
        this.orgTree = res.data
      })
    },
    methods: {
      init() {
        this.visible = true
        const moduleOption = this.$options
        this.moduleData = moduleOption.filters['dictData']('issue_module')
        const consequenceOption = this.$options
        this.consequenceData = consequenceOption.filters['dictData']('issue_consequence')
        const issueClassificationOption = this.$options
        this.issueClassificationData = issueClassificationOption.filters['dictData']('issue_classification')
        const sourceOption = this.$options
        this.sourceData = sourceOption.filters['dictData']('issue_source')
        // const statusOption = this.$options
        // this.statusData = statusOption.filters['dictData']('issue_status')
      },
      customRequest(data) {
        this.fileObj = data.file
      },
      // 初始化方法
      add () {
        this.init()

        this.form.resetFields()

        this.discoverTimeDateString = ''
        this.discoverTime = ''
      },
      copy (record) {
        this.init()

        setTimeout(() => {
          this.form.setFieldsValue(
            {
              title: record.title,
              projectId: record.projectId,
              productId: record.productId,
              module: record.module,
              consequence: record.consequence,
              issueClassification: record.issueClassification,
              source: record.source,
              // status: record.status,
              // creatorId: record.creatorId,
              discover: record.discover,
              dispatcher: record.dispatcher,
              cC: record.copyTo
              // executor: record.executor,
              // verifier: record.verifier,
              // verifierPlace: record.verifierPlace
              // description: this.description
            }
          )
        }, 30)
        this.form.getFieldDecorator('createTime', { initialValue: moment(record.createTime, 'YYYY-MM-DD') })
        this.createTimeDateString = moment(record.createTime).format('YYYY-MM-DD')
        this.form.getFieldDecorator('closeTime', { initialValue: moment(record.closeTime, 'YYYY-MM-DD') })
        this.closeTimeDateString = moment(record.closeTime).format('YYYY-MM-DD')
        this.form.getFieldDecorator('discoverTime', { initialValue: moment(record.discoverTime, 'YYYY-MM-DD') })
        this.discoverTimeDateString = moment(record.discoverTime).format('YYYY-MM-DD')
        this.form.getFieldDecorator('dispatchTime', { initialValue: moment(record.dispatchTime, 'YYYY-MM-DD') })
        this.dispatchTimeDateString = moment(record.dispatchTime).format('YYYY-MM-DD')
        this.form.getFieldDecorator('forecastSolveTime', { initialValue: moment(record.forecastSolveTime, 'YYYY-MM-DD') })
        this.forecastSolveTimeDateString = moment(record.forecastSolveTime).format('YYYY-MM-DD')
        this.form.getFieldDecorator('solveTime', { initialValue: moment(record.solveTime, 'YYYY-MM-DD') })
        this.solveTimeDateString = moment(record.solveTime).format('YYYY-MM-DD')
        this.form.getFieldDecorator('validateTime', { initialValue: moment(record.validateTime, 'YYYY-MM-DD') })
        this.validateTimeDateString = moment(record.validateTime).format('YYYY-MM-DD')
      },
      /**
       * 提交表单
       */
      handleSubmit () {
        const { form: { validateFields } } = this
        this.confirmLoading = true
        validateFields((errors, values) => {
          if (!errors) {
            for (const key in values) {
              if (typeof (values[key]) === 'object') {
                values[key] = JSON.stringify(values[key])
              }
            }
            values.createTime = this.createTimeDateString
            values.closeTime = this.closeTimeDateString
            values.discoverTime = this.discoverTimeDateString
            values.dispatchTime = this.dispatchTimeDateString
            values.forecastSolveTime = this.forecastSolveTimeDateString
            values.solveTime = this.solveTimeDateString
            values.validateTime = this.validateTimeDateString

            IssueAdd(values).then((res) => {
              if (res.success) {
                var issueId = res.data

                if (this.fileObj) {
                  const formData = new FormData()
                  formData.append('file', this.fileObj)
                  sysFileInfoUpload(formData).then((res) => {
                    if (res.success) {
                      this.$message.success('上传成功')
                      // this.$refs.table.refresh()

                      // 后端将附件Id和问题Id关联
                      var attachmentId = res.data
                      var fileName = this.fileObj.fileName
                      // 0：正常附件 1：问题详情富文本 2：原因分析富文本 3：解决措施富文本 4：验证情况富文本
                      var attachmentType = 0

                      var data = {
                        attachment:{
                          attachmentId: attachmentId,
                          fileName: fileName,
                          attachmentType: attachmentType
                        },
                        issueId: issueId
                      }

                      IssueAttachmentSaveId(data).then((res) => {
                        if (!res.success){
                          this.$message.error('附件信息保存失败：' + res.message)
                        } else {
                          this.fileObj = ''
                        }
                      })

                    } else {
                      this.$message.error('上传失败：' + res.message)
                    }
                  })
                }

                this.$message.success('新增成功')
                this.confirmLoading = false
                this.$emit('ok', values)
                this.handleCancel()
              } else {
                this.$message.error('新增失败：' + JSON.stringify(res.message))
              }
            }).finally((res) => {
              this.confirmLoading = false
            })
          } else {
            this.confirmLoading = false
          }
        })
      },
      onChangecreateTime(date, dateString) {
        this.createTimeDateString = dateString
      },
      onChangecloseTime(date, dateString) {
        this.closeTimeDateString = dateString
      },
      onChangediscoverTime(date, dateString) {
        this.discoverTimeDateString = dateString
      },
      onChangedispatchTime(date, dateString) {
        this.dispatchTimeDateString = dateString
      },
      onChangeforecastSolveTime(date, dateString) {
        this.forecastSolveTimeDateString = dateString
      },
      onChangesolveTime(date, dateString) {
        this.solveTimeDateString = dateString
      },
      onChangevalidateTime(date, dateString) {
        this.validateTimeDateString = dateString
      },
      handleCancel () {
        this.form.resetFields()
        this.visible = false
      }
    }
  }
</script>
