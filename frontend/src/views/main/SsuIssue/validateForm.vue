<template>
  <a-modal
    title="验证问题"
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
        <a-form-item label="验证数量" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input-number placeholder="请输入验证数量" style="width: 100%" v-decorator="['count',{rules: [{required:true, message: '请输入验证数量！'}]}]" />
        </a-form-item>
        <a-form-item label="批次" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入批次" v-decorator="['batch', {rules: [{required: true, message: '请输入批次！'}]}]" />
        </a-form-item>
        <a-form-item label="验证情况" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入验证情况" v-decorator="['result',{rules: [{required:true, message: '请输入验证情况！'}]}]" />
        </a-form-item>
        <a-form-item label="验证地点" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入验证地点" v-decorator="['verifierPlace',{rules: [{required:true, message: '请输入验证地点！'}]}]" />
        </a-form-item>

        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="验证通过">
          <a-switch
            id="passResult"
            checkedChildren="是"
            unCheckedChildren="否"
            v-decorator="['passResult', { valuePropName: 'checked' }]"
          />
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
  SsuIssueUploadFile,
  SsuIssueValidate
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
      operationRecords: ''
    }
  },
  methods: {
    moment,
    customRequest(data) {
      const formData = new FormData()
      formData.append('file', data.file)
      SsuIssueUploadFile(formData).then((res) => {
        if (res.success) {
          this.$message.success('上传成功')
          this.$refs.table.refresh()
        } else {
          this.$message.error('上传失败：' + res.message)
        }
      })
    },
    // 初始化方法
    edit (record) {
      this.visible = true
      this.record = record

      setTimeout(() => {
        this.form.setFieldsValue(
          {
            id: record.id,
            title: record.title,
            passResult: true
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
          SsuIssueValidate(this.record).then((res) => {
            if (res.success) {
              this.$message.success('验证完成')
              this.confirmLoading = false
              this.$emit('ok', this.record)
              this.handleCancel()
            } else {
              this.$message.error('验证失败：' + JSON.stringify(res.message))
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
    }
  }
}
</script>
