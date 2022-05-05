<template>
  <a-modal
    title="分发问题"
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

        <a-form-item label="问题分类" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select style="width: 100%" placeholder="请选择问题分类" v-decorator="['issueClassification', {rules: [{ required: true, message: '请选择问题分类！' }]}]">
            <a-select-option v-for="(item,index) in issueClassificationData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
          </a-select>
        </a-form-item>

        <a-form-item label="问题性质" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select style="width: 100%" placeholder="请选择问题性质" v-decorator="['consequence', {rules: [{ required: true, message: '请选择问题性质！' }]}]">
            <a-select-option v-for="(item,index) in consequenceData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
          </a-select>
        </a-form-item>

        <a-form-item label="执行人" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input-number placeholder="请输入执行人" style="width: 100%" v-decorator="['executor', {rules: [{ required: true, message: '请选择执行人！' }]}]" />
        </a-form-item>

        <a-form-item label="预计完成日期" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-date-picker style="width: 100%" placeholder="请选择预计完成日期" v-decorator="['forecastSolveTime']" @change="onChangeforecastSolveTime"/>
        </a-form-item>

        <a-form-item label="被抄送人" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input-number placeholder="请输入被抄送人" style="width: 100%" v-decorator="['cC']" />
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
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      consequenceData: [],
      issueClassificationData: [],
      forecastSolveTimeDateString: ''
    }
  },
  created() {
    const consequenceOption = this.$options
    this.consequenceData = consequenceOption.filters['dictData']('issue_consequence')
    const issueClassificationOption = this.$options
    this.issueClassificationData = issueClassificationOption.filters['dictData']('issue_classification')
  },
  methods: {
    moment,
    // 初始化方法
    edit (record) {
      this.visible = true
      this.record = record

      setTimeout(() => {
        this.form.setFieldsValue(
          {
            id: record.id,
            title: record.title,
            consequence: record.consequence,
            issueClassification: record.issueClassification,
            executor: record.executor,
            forecastSolveTime: record.forecastSolveTime,
            cC: record.copyTo
          }
        )
      }, 100)
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

          values.forecastSolveTime = this.forecastSolveTimeDateString
          this.record.forecastSolveTime = this.forecastSolveTimeDateString

          SsuIssueDispatch(this.record).then((res) => {
            if (res.success) {
              this.$message.success('分发成功')
              this.confirmLoading = false
              this.$emit('ok', this.record)
              this.handleCancel()
            } else {
              this.$message.error('分发失败：' + JSON.stringify(res.message))
            }
          }).finally((res) => {
            this.confirmLoading = false
          })
        } else {
          this.confirmLoading = false
        }
      })
    },
    handleCancel () {
      this.form.resetFields()
      this.visible = false
    },
    onChangeforecastSolveTime(date, dateString) {
      this.forecastSolveTimeDateString = dateString
    }
  }
}
</script>
