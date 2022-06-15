<!--
 * @Author: 林伟群
 * @Date: 2022-05-31 20:18:02
 * @LastEditTime: 2022-06-15 16:32:39
 * @LastEditors: 林伟群
 * @Description: 批量转交
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\ProblemBatchRedispatch.vue
-->
<template>
  <a-modal v-model="visible" title="问题批量转交" on-ok="handleOk">
    <section class="form_1">
      <a-form-model ref="ruleForm" :labelCol="labelCol" :wrapperCol="wrapperCol" :model="form" :rules="rules">
        <a-form-model-item ref="executorName" label="转交人" prop="executorName">
          <section class="from_chilen">
            <!-- <a-input v-model="form.executorName" placeholder="请选择转交人" disabled /> -->
            <SelectUser title="请输入转交人" @handlerSelectUser="handlerSelectUser" :userSelect="userSelect"></SelectUser>
            <a-button @click="changePersonnel('batcheExecutor')"> 选择 </a-button>
          </section>
        </a-form-model-item>
        <a-form-model-item label="备注" prop="comment">
          <a-textarea v-model="form.comment" :rows="4" placeholder="请输入备注" />
        </a-form-model-item>
      </a-form-model>
    </section>
    <template slot="footer">
      <a-button @click="handleSubmit" type="primary"> 确定 </a-button>
      <a-button @click="handleCancel"> 取消 </a-button>
    </template>
  </a-modal>
</template>

<script>
import { IssueRedispatch } from '@/api/modular/main/SsuIssueManage'
import SelectUser from './SelectUser.vue'

export default {
  components: {
    SelectUser,
  },
  inject: ['getProblemList'],
  data() {
    return {
      labelCol: { md: { span: 24 }, lg: { span: 5 } },
      wrapperCol: { md: { span: 24 }, lg: { span: 18 } },
      form: {
        executor: null, // 解决ID
        executorName: '', // 解决人
        comment: '', // 备注
      },
      checkRecord: [],
      rules: {
        executorName: [{ required: true, message: '请选择解决人', trigger: 'blur' }],
      },
      visible: false,
    }
  },
  computed: {
    userSelect() {
      return {
        id: this.form.executor,
        name: this.form.executorName,
      }
    },
  },
  methods: {
    initRedispatch(record) {
      this.visible = true
      this.checkRecord = record
    },

    // 模糊搜索选中人员
    handlerSelectUser(valueObj) {
      this.form.executor = valueObj.value
      this.form.executorName = valueObj.label
    },
    // 人员选择
    changePersonnel(value) {
      this.$parent.userVisible = !this.$parent.userVisible
      this.$emit('changePersonnel', value)
    },
    // 确定
    handleSubmit() {
      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          const { executorName, executor, comment } = this.form
          const parameterArray = this.checkRecord.map((item) => {
            const { id, title } = item
            const newItem = { id, title, executor, comment }
            return newItem
          })
          IssueRedispatch(parameterArray)
            .then((res) => {
              if (res.success) {
                this.$message.success('问题转交成功')
                this.visible = false
                this.getProblemList()
              } else {
                this.$message.warning(res.message)
              }
            })
            .catch(() => {
              this.$message.error('问题转交失败')
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