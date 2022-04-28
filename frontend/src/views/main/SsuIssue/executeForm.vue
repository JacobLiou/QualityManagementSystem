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

        <a-form-item label="解决日期" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-date-picker style="width: 100%" placeholder="请选择解决日期" v-decorator="['solveTime',{rules: [{readonly:true}]}]" @change="onChangesolveTime"/>
        </a-form-item>

        <a-form-item label="解决版本" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select style="width: 100%" placeholder="请选择解决版本" v-decorator="['solveVersion', {rules: [{readonly:true, required: true, message: '请选择问题来源！' }]}]">
            <a-select-option v-for="(item,index) in solveVersioneData" :key="index" :value="item.code">{{ item.name }}</a-select-option>
          </a-select>
        </a-form-item>

        <a-form-item label="原因分析" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入原因分析" v-decorator="['reason', {rules: [{required: true, message: '请输入原因分析！'}]}]" />
        </a-form-item>

        <a-form-item label="解决措施" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入解决措施" v-decorator="['measures', {rules: [{required: true, message: '请输入解决措施！'}]}]" />
        </a-form-item>

        <a-upload
          :customRequest="customRequest"
          :multiple="true"
          :showUploadList="false"
          name="file"
          v-if="hasPerm('sysUser:import')">
          <a-button icon="up-circle">附件上传</a-button>
        </a-upload>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import moment from 'moment'
import {
  SsuIssueExecute
} from '@/api/modular/main/SsuIssueManage'
import { sysUserImport } from '@/api/modular/system/userManage'
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
      solveVersioneData: []
    }
  },
  methods: {
    moment,
    created() {
      this.solveVersioneData = this.$options.filters['dictData']('issue_solve_version')

      console.log(this.solveVersioneData)
    },
    edit(record) {
      this.visible = true
      this.record = record
    },
    customRequest(data) {
      const formData = new FormData()
      formData.append('file', data.file)
      sysUserImport(formData).then((res) => {
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
