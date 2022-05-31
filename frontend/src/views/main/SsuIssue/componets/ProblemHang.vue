<!--
 * @Author: 林伟群
 * @Date: 2022-05-31 14:51:28
 * @LastEditTime: 2022-05-31 17:15:15
 * @LastEditors: 林伟群
 * @Description: 问题挂起
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\ProblemHang.vue
-->
<template>
  <a-modal v-model="visible" title="问题挂起" on-ok="handleOk">
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
        <a-form-model-item label="挂起情况" ref="hangupReason" prop="hangupReason">
          <a-textarea v-model="form.hangupReason" :rows="4" placeholder="请输入备注" />
        </a-form-model-item>
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
import { IssueHangup } from '@/api/modular/main/SsuIssueManage'
import OperRecords from './OperRecords.vue'
export default {
  components: {
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
        hangupReason: '', // 挂起情况
      },
      rules: {
        title: [{ required: true, message: '请输入问题简述', trigger: 'blur' }],
        hangupReason: [{ required: true, message: '请输入挂起情况', trigger: 'blur' }],
      },
      visible: false,
      isShow: true,
    }
  },
  methods: {
    initHang(record, isShow = true) {
      this.visible = true
      this.form.id = record.id
      this.form.title = record.title
      this.isShow = isShow
    },
    // 确定
    handleSubmit() {
      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          IssueHangup(this.form)
            .then((res) => {
              if (res.success) {
                this.$message.success('问题挂起成功')
                this.visible = false
                this.getProblemList()
              } else {
                this.$message.warning(res.message)
              }
            })
            .catch(() => {
              this.$message.error('问题挂起失败')
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