<!--
 * @Author: 林伟群
 * @Date: 2022-05-31 10:51:13
 * @LastEditTime: 2022-05-31 17:12:52
 * @LastEditors: 林伟群
 * @Description: 问题验证组件
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\ProblemValidate.vue
-->
<template>
  <a-modal v-model="visible" title="问题验证" on-ok="handleOk">
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
        <a-form-model-item ref="count" label="验证数量" prop="count">
          <a-input-number placeholder="请输入验证数量" style="width: 100%" v-model="form.count" :min="0" />
        </a-form-model-item>
        <a-form-model-item ref="batch" label="验证批次" prop="batch">
          <a-input
            v-model="form.batch"
            @blur="
              () => {
                $refs.batch.onFieldBlur()
              }
            "
            placeholder="请输入验证批次"
          />
        </a-form-model-item>
        <a-form-model-item label="验证地点" prop="verifierPlace">
          <a-textarea v-model="form.verifierPlace" :rows="4" placeholder="请输入验证情况" />
        </a-form-model-item>
        <a-form-model-item label="验证情况" prop="result">
          <a-textarea v-model="form.result" :rows="4" placeholder="请输入验证情况" />
        </a-form-model-item>
        <a-form-item label="验证通过">
          <a-switch v-model="form.passResult" checkedChildren="是" unCheckedChildren="否" />
        </a-form-item>
        <a-form-item label="附件上传">
          <ProblemUplod @uploadProblem="uploadProblem"></ProblemUplod>
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
import { IssueAttachmentSaveId, IssueValidate } from '@/api/modular/main/SsuIssueManage'
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
      form: {
        id: null,
        title: '',
        verifierPlace: '', // 验证地点
        passResult: false, // 是否通过验证
        count: 0, // 验证数量
        batch: '', // 验证批次
        result: '', // 验证情况
      },
      rules: {
        title: [{ required: true, message: '请输入问题简述', trigger: 'blur' }],
        batch: [{ required: true, message: '请输入验证批次', trigger: 'blur' }],
        count: [{ required: true, message: '请输入验证数量', trigger: 'blur' }],
      },
      visible: false,
      attachment: {}, // 附件上次的数据
      isShow: true,
    }
  },
  methods: {
    initValidate(record, isShow = true) {
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
          parameter.passResult = parameter.passResult ? 1 : 0
          IssueValidate(parameter)
            .then((res) => {
              if (res.success) {
                // const issueId = res.data.id
                // const parameter = {
                //   attachment: this.attachment,
                //   issueId: issueId,
                // }
                // IssueAttachmentSaveId(parameter)
                //   .then((res) => {
                //     if (!res.success) {
                //       this.$message.error('附件信息保存失败：' + res.message)
                //     }
                //   })
                //   .catch(() => {
                //     this.$message.error('附件信息保存失败：' + res.message)
                //   })
                this.$message.success('问题验证成功')
                this.visible = false
                this.getProblemList()
              } else {
                this.$message.warning(res.message)
              }
            })
            .catch(() => {
              this.$message.error('问题验证失败')
            })
        } else {
          return false
        }
      })
    },
    // 取消
    handleCancel() {
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