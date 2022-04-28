<template>
  <a-modal
    title="编辑问题记录"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item v-show="false"><a-input v-decorator="['id']" /></a-form-item>
        <a-form-item label="问题简述" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入问题简述" v-decorator="['title', {rules: [{required: true, message: '请输入问题简述！'}]}]" />
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import moment from 'moment'
import {
  SsuIssueDispatch
} from '@/api/modular/main/SsuIssueManage'
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
      record: {},
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
      form: this.$form.createForm(this)
    }
  },
  methods: {
    moment,
    // 初始化方法
    edit (record) {
      this.visible = true
      this.record = record
      const moduleOption = this.$options
      this.moduleData = moduleOption.filters['dictData']('issue_module')
      const consequenceOption = this.$options
      this.consequenceData = consequenceOption.filters['dictData']('issue_consequence')
      const issueClassificationOption = this.$options
      this.issueClassificationData = issueClassificationOption.filters['dictData']('issue_classification')
      const sourceOption = this.$options
      this.sourceData = sourceOption.filters['dictData']('issue_source')
      const statusOption = this.$options
      this.statusData = statusOption.filters['dictData']('isssue_status')
    },
    handleSubmit () {
      const { form: { validateFields } } = this
      this.confirmLoading = true
      validateFields((errors, values) => {
        if (!errors) {
          for (const key in values) {
            if (values[key] == null) continue
            if (typeof (values[key]) === 'object') {
              values[key] = JSON.stringify(values[key])
              this.record[key] = values[key]
            } else {
              this.record[key] = values[key]
            }
          }
          values.createTime = this.createTimeDateString
          this.record.createTime = this.createTimeDateString
          values.closeTime = this.closeTimeDateString
          this.record.closeTime = this.closeTimeDateString
          values.discoverTime = this.discoverTimeDateString
          this.record.discoverTime = this.discoverTimeDateString
          values.dispatchTime = this.dispatchTimeDateString
          this.record.dispatchTime = this.dispatchTimeDateString
          values.forecastSolveTime = this.forecastSolveTimeDateString
          this.record.forecastSolveTime = this.forecastSolveTimeDateString
          values.solveTime = this.solveTimeDateString
          this.record.solveTime = this.solveTimeDateString
          values.validateTime = this.validateTimeDateString
          this.record.validateTime = this.validateTimeDateString


          SsuIssueDispatch(JSON.stringify(this.record)).then((res) => {
            if (res.success) {
              this.$message.success('编辑成功')
              this.confirmLoading = false
              this.$emit('ok', this.record)
              this.handleCancel()
            } else {
              this.$message.error('编辑失败：' + JSON.stringify(res.message))
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
