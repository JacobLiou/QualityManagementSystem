<template>
  <a-modal
    title="解决问题"
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

        <a-form-item label="解决日期" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-date-picker style="width: 100%" placeholder="请选择解决日期" v-decorator="['solveTime']" @change="onChangesolveTime"/>
        </a-form-item>

        <a-form-item label="解决版本" :labelCol="labelCol" :wrapperCol="wrapperCol">
<!--            <a-input placeholder="请输入问题简述" v-decorator="['solveVersion', {rules: [{required: true, message: '请输入解决版本！'}]}]" />-->
          <a-select style="width: 100%" placeholder="请选择解决版本" v-decorator="['solveVersion', {rules: [{required: true, message: '请选择解决版本！' }]}]">
            <a-select-option v-for="(item,index) in solveVersionData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
          </a-select>
        </a-form-item>

        <a-form-item label="原因分析" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入原因分析" v-decorator="['reason', {rules: [{required: true, message: '请输入原因分析！'}]}]" />
        </a-form-item>

        <a-form-item label="解决措施" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入解决措施" v-decorator="['measures', {rules: [{required: true, message: '请输入解决措施！'}]}]" />
        </a-form-item>

        <a-form-item label="附件上传" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-upload
            :customRequest="customRequest"
            :multiple="true"
            :showUploadList="false"
            name="file"
            v-if="hasPerm('sysUser:import')">
            <a-button icon="upload">附件上传</a-button>
          </a-upload>
        </a-form-item>
      </a-form>
    </a-spin>

    <div>
      <strong>历史记录</strong>

      <ul  v-if="this.operationRecords!=''">
        <li v-for="(item, index) in operationRecords" :key="index" :value="item.operationTypeId">
          {{index+1}}. {{item.operationTime}}, 由 <b>{{item.operatorName}}</b> {{'issue_operation_type'| dictType(item.operationTypeId)}}
        </li>
      </ul>
    </div>
  </a-modal>
</template>

<script>
import moment from 'moment'
import {
  OperationPage,
  SsuIssueExecute, SsuIssueUploadFile
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
      solveTimeDateString: '',
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      solveVersionData: [],
      operationRecords: ''
    }
  },
  methods: {
    moment,
    edit(record) {
      this.visible = true
      this.record = record

      this.solveVersionData = this.$options.filters['dictData']('issue_solve_version')

      setTimeout(() => {
        this.form.setFieldsValue(
          {
            id: record.id,
            title: record.title
          }
        )
      }, 100)

      this.record.issueId = this.record.id
      OperationPage(this.record).then((res) => {
        if (res.success) {
          this.operationRecords = res.data.rows
        } else {
          this.$message.error('问题操作记录读取失败')
        }
      }).finally((res) => {
        this.confirmLoading = false
      })
    },
    customRequest(data) {
      const formData = new FormData()
      formData.append('file', data.file)
      SsuIssueUploadFile(formData).then((res) => {
        if (res.success) {
          this.$message.success('上传成功')
          // this.$refs.table.refresh()
        } else {
          this.$message.error('上传失败：' + res.message)
        }
      })
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
          values.solveTime = this.solveTimeDateString
          this.record.solveTime = this.solveTimeDateString
          SsuIssueExecute(this.record).then((res) => {
            if (res.success) {
              this.$message.success('处理成功')
              this.confirmLoading = false
              this.$emit('ok', this.record)
              this.handleCancel()
            } else {
              this.$message.error('处理失败：' + JSON.stringify(res.message))
            }
          }).finally((res) => {
            this.confirmLoading = false
          })
        } else {
          this.confirmLoading = false
        }
      })
    },
    onChangesolveTime(date, dateString) {
      this.solveTimeDateString = dateString
    },
    handleCancel () {
      this.form.resetFields()
      this.visible = false
    }
  }
}
</script>
