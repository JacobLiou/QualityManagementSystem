<template>
  <a-modal
    title="新增详细问题记录"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item label="问题详情" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-textarea placeholder="请输入问题详情" v-decorator="['description']" :auto-size="{ minRows: 3, maxRows: 6 }"/>
        </a-form-item>
        <a-form-item label="原因分析" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-textarea placeholder="请输入原因分析" v-decorator="['reason']" :auto-size="{ minRows: 3, maxRows: 6 }"/>
        </a-form-item>
        <a-form-item label="解决措施" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-textarea placeholder="请输入解决措施" v-decorator="['measures']" :auto-size="{ minRows: 3, maxRows: 6 }"/>
        </a-form-item>
        <a-form-item label="验证数量" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input-number placeholder="请输入验证数量" style="width: 100%" v-decorator="['count']" />
        </a-form-item>
        <a-form-item label="验证批次" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入验证批次" v-decorator="['batch']" />
        </a-form-item>
        <a-form-item label="验证情况" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-textarea placeholder="请输入验证情况" v-decorator="['result']" :auto-size="{ minRows: 3, maxRows: 6 }"/>
        </a-form-item>
        <a-form-item label="解决版本" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input placeholder="请输入解决版本" v-decorator="['solveVersion']" />
        </a-form-item>
        <a-form-item label="备注" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-textarea placeholder="请输入备注" v-decorator="['comment']" :auto-size="{ minRows: 3, maxRows: 6 }"/>
        </a-form-item>
        <a-form-item label="挂起情况" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-textarea placeholder="请输入挂起情况" v-decorator="['hangupReason']" :auto-size="{ minRows: 3, maxRows: 6 }"/>
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
  import {
    SsuIssueDetailAdd
  } from '@/api/modular/main/SsuIssueDetailManage'

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
        visible: false,
        confirmLoading: false,
        form: this.$form.createForm(this)
      }
    },
    methods: {
      // 初始化方法
      add (record) {
        this.visible = true
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
            SsuIssueDetailAdd(values).then((res) => {
              if (res.success) {
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
      handleCancel () {
        this.form.resetFields()
        this.visible = false
      }
    }
  }
</script>
