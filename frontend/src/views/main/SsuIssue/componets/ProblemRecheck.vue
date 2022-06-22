<!--
 * @Author: 林伟群
 * @Date: 2022-05-30 17:12:55
 * @LastEditTime: 2022-06-22 15:21:00
 * @LastEditors: 林伟群
 * @Description: 问题复核
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\ProblemRecheck.vue
-->
<template>
  <a-modal v-model="visible" title="复核问题" on-ok="handleOk">
    <section class="form_1">
      <a-form-model ref="ruleForm" :labelCol="labelCol" :wrapperCol="wrapperCol" :model="form" :rules="rules">
        <a-form-model-item ref="title" label="问题简述" prop="title">
          <a-input
            v-model="form.title"
            @blur="
              () => {
                $refs.title.onFieldBlur()
              }
            "
            placeholder="请输入问题简述"
          />
        </a-form-model-item>
        <a-form-model-item ref="reCheckResult" label="复核情况" prop="reCheckResult">
          <a-textarea
            v-model="form.reCheckResult"
            @blur="
              () => {
                $refs.reCheckResult.onFieldBlur()
              }
            "
            :rows="4"
            placeholder="请输入复核情况"
          />
        </a-form-model-item>
        <a-form-model-item label="复核状态" prop="passResult">
          <a-radio-group v-model="form.passResult">
            <a-radio value="0"> 复核不通过 </a-radio>
            <a-radio value="1"> 复核通过 </a-radio>
          </a-radio-group>
        </a-form-model-item>
        <a-form-item label="附件上传">
          <ProblemUplod @uploadProblem="uploadProblem" :type="2"></ProblemUplod>
        </a-form-item>
      </a-form-model>
      <OperRecords :id="form.id" isModal v-if="isShow"></OperRecords>
    </section>
    <template slot="footer">
      <a-button @click="handleSubmit" type="primary"> 确定 </a-button>
      <a-button @click="handleCancel"> 取消 </a-button>
    </template>
  </a-modal>
</template>

<script>
import ProblemUplod from './ProblemUplod.vue'
import { IssueAttachmentSaveId, IssueReCheck } from '@/api/modular/main/SsuIssueManage'
import OperRecords from './OperRecords.vue'
export default {
  components: {
    ProblemUplod,
    OperRecords,
  },
  inject: ['getProblemList'],
  data() {
    return {
      labelCol: { md: { span: 24 }, lg: { span: 5 } },
      wrapperCol: { md: { span: 24 }, lg: { span: 18 } },
      labelCol2: { md: { span: 6 }, lg: { span: 6 }, xs: { span: 8 } },
      form: {
        id: null,
        title: '', // 问题简述，
        passResult: '', // 复核状态
        reCheckResult: '', // 复核情况
      },
      rules: {
        title: [{ required: true, message: '请输入问题简述', trigger: 'blur' }],
        reCheckResult: [{ required: true, message: '请输入原因分析', trigger: 'blur' }],
        passResult: [{ required: true, message: '请选择复核状态', trigger: 'change' }],
      },
      visible: false,
      attachment: [], // 附件上传的数据
      isShow: true,
    }
  },
  methods: {
    // 开启复核弹窗
    recheckForm(record, isShow = true) {
      this.visible = true
      this.form.id = record.id
      this.form.title = record.title
      this.isShow = isShow
    },
    // 附件上传
    uploadProblem() {
      this.attachment = val
    },

    // 确定
    handleSubmit() {
      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          const parameter = JSON.parse(JSON.stringify(this.form))
          parameter.passResult = Number(parameter.passResult)
          IssueReCheck(parameter)
            .then((res) => {
              if (res.success) {
                // 保存ID
                if (this.attachment.length !== 0) {
                  const parameter = {
                    attachments: this.attachment,
                    issueId: this.form.id,
                  }
                  IssueAttachmentSaveId(parameter)
                    .then((res) => {
                      if (!res.success) {
                        this.$message.error('附件信息保存失败：' + res.message)
                      }
                    })
                    .catch(() => {
                      this.$message.error('附件信息保存失败：' + res.message)
                    })
                }
                this.$message.success('问题复核成功')
                this.getProblemList()
                this.handleCancel()
              } else {
                this.$message.warning(res.message)
              }
            })
            .catch(() => {
              this.$message.error('问题复核失败')
            })
        } else {
          return false
        }
      })
    },
    // 取消
    handleCancel() {
      Object.assign(this, {
        form: {
          id: null,
          title: '', // 问题简述，
          passResult: '', // 复核状态
          reCheckResult: '', // 复核情况
        },
        attachment: [],
      })
      this.visible = false
    },
  },
}
</script>

<style lang="less" scoped>
.form_1 {
  /deep/.ant-row {
    display: flex;
    align-items: flex-start;
    flex-wrap: wrap;
  }
  .from_chilen {
    display: flex;
  }
}
</style>