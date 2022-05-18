<template>
  <a-modal
    title="复核问题"
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
        <a-form-item label="复核情况" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-textarea
            :rows="4"
            placeholder="请输入复核情况"
            v-decorator="['reCheckResult']"></a-textarea>
        </a-form-item>

        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="复核通过">
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
            :showUploadList="true"
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
  IssueOperationPage,
  IssueAttachmentSaveId, IssueReCheck
} from '@/api/modular/main/SsuIssueManage'
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
      this.fileObj = data.file
    },
    // 初始化方法
    edit (record) {
      this.visible = true
      this.record = record

      setTimeout(() => {
        this.form.setFieldsValue(
          {
            id: record.id,
            title: record.title
          }
        )
      }, 100)

      this.record.issueId = this.record.id
      IssueOperationPage(this.record).then((res) => {
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

          IssueReCheck(this.record).then((res) => {
            if (res.success) {
              if (this.fileObj) {
                const formData = new FormData()
                formData.append('file', this.fileObj)
                sysFileInfoUpload(formData).then((res) => {
                  if (res.success) {
                    this.$message.success('上传成功')
                    // this.$refs.table.refresh()

                    // 后端将附件Id和问题Id关联
                    var attachmentId = res.data
                    var fileName = this.fileObj.name
                    // 0：正常附件 1：问题详情富文本 2：原因分析富文本 3：解决措施富文本 4：验证情况富文本
                    var attachmentType = 0

                    var data = {
                      attachment: [{
                        attachmentId: attachmentId,
                        fileName: fileName,
                        attachmentType: attachmentType
                      }],
                      issueId: this.record.issueId
                    }

                    IssueAttachmentSaveId(data).then((res) => {
                      if (!res.success) {
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

              this.$message.success('挂起成功')
              this.confirmLoading = false
              this.$emit('ok', this.record)
              this.handleCancel()
            } else {
              this.$message.error('挂起失败：' + JSON.stringify(res.message))
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
