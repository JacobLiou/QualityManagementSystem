<!--
 * @Author: 林伟群
 * @Date: 2022-05-14 14:24:22
 * @LastEditTime: 2022-06-22 14:52:28
 * @LastEditors: 林伟群
 * @Description: 挂起问题
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\HangProblem.vue
-->
<template>
  <a-modal
    title="挂起问题"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel"
  >
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item v-show="false"><a-input v-decorator="['id']" /></a-form-item>
        <a-form-item label="问题简述" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input
            placeholder="请输入问题简述"
            v-decorator="['title', { rules: [{ required: true, message: '请输入问题简述！' }] }]"
          />
        </a-form-item>
        <a-form-item label="挂起情况" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input
            placeholder="请输入挂起情况"
            v-decorator="['hangupReason', { rules: [{ required: true, message: '请输入挂起情况！' }] }]"
          />
        </a-form-item>
      </a-form>
    </a-spin>

    <div>
      <strong>历史记录</strong>
      <ul v-if="this.operationRecords != ''">
        <li v-for="(item, index) in operationRecords" :key="index" :value="item.operationTypeId">
          {{ index + 1 }}. {{ item.operationTime }}, 由 <b>{{ item.operatorName }}</b>
          {{ 'issue_operation_type' | dictType(item.operationTypeId) }}
        </li>
      </ul>
    </div>
  </a-modal>
</template>

<script>
import moment from 'moment'
import { OperationPage, IssueHangup } from '@/api/modular/main/SsuIssueManage'
export default {
  data() {
    return {
      labelCol: {
        xs: { span: 24 },
        sm: { span: 5 },
      },
      wrapperCol: {
        xs: { span: 24 },
        sm: { span: 15 },
      },
      record: {},
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      operationRecords: '',
    }
  },
  methods: {
    moment,
    // 初始化方法
    edit(record) {
      this.visible = true
      this.record = record

      setTimeout(() => {
        this.form.setFieldsValue({
          id: record.id,
          title: record.title,
        })
      }, 100)

      this.record.issueId = this.record.id
      OperationPage(this.record)
        .then((res) => {
          if (res.success) {
            this.operationRecords = res.data.rows
          } else {
            this.$message.error('问题操作记录读取失败')
          }
        })
        .finally((res) => {
          this.confirmLoading = false
        })
    },
    handleSubmit() {
      const {
        form: { validateFields },
      } = this
      this.confirmLoading = true
      validateFields((errors, values) => {
        if (!errors) {
          for (const key in values) {
            if (values[key] == null) continue
            if (typeof values[key] === 'object') {
              values[key] = JSON.stringify(values[key])
              this.record[key] = values[key]
            } else {
              this.record[key] = values[key]
            }
          }

          IssueHangup(this.record)
            .then((res) => {
              if (res.success) {
                this.$message.success('挂起成功')
                this.confirmLoading = false
                this.$emit('ok', this.record)
                this.handleCancel()
              } else {
                this.$message.error('挂起失败：' + JSON.stringify(res.message))
              }
            })
            .finally((res) => {
              this.confirmLoading = false
            })
        } else {
          this.confirmLoading = false
        }
      })
    },
    handleCancel() {
      console.log(111111111111);
      this.form.resetFields()
      this.form.setFieldsValue({
        title: '',
        hangupReason: '',
      })
      this.visible = false
    },
  },
}
</script>
