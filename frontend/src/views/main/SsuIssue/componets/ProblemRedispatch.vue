<!--
 * @Author: 林伟群
 * @Date: 2022-05-31 11:30:01
 * @LastEditTime: 2022-06-22 15:21:23
 * @LastEditors: 林伟群
 * @Description: 问题转交
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\ProblemRedispatch.vue
-->
<template>
  <a-modal v-model="visible" title="问题转交" on-ok="handleOk">
    <section class="form_1">
      <a-form-model ref="ruleForm" :labelCol="labelCol" :wrapperCol="wrapperCol" :model="form" :rules="rules">
        <a-form-model-item ref="title" label="问题简述" prop="title">
          <a-textarea
            v-model="form.title"
            @blur="
              () => {
                $refs.title.onFieldBlur()
              }
            "
            minlength="1"
            placeholder="请输入问题简述"
          />
        </a-form-model-item>
        <a-form-model-item ref="currentAssignmentName" label="转交人" prop="currentAssignmentName">
          <section class="from_chilen">
            <!-- 二次封装远程搜索组件 -->
            <SelectUser
              title="请输入转交人"
              @handlerSelectUser="handlerSelectUser"
              :userSelect="userSelect"
            ></SelectUser>
            <a-button @click="changePersonnel('currentAssignment')"> 选择 </a-button>
          </section>
        </a-form-model-item>
        <a-form-model-item label="备注" prop="comment">
          <a-textarea v-model="form.comment" :rows="6" placeholder="请输入备注" />
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
import { IssueRedispatch } from '@/api/modular/main/SsuIssueManage'
import OperRecords from './OperRecords.vue'
import SelectUser from './SelectUser.vue'

export default {
  components: {
    OperRecords,
    SelectUser,
  },
  inject: ['getProblemList'],
  data() {
    return {
      labelCol: { md: { span: 24 }, lg: { span: 5 } },
      wrapperCol: { md: { span: 24 }, lg: { span: 18 } },
      form: {
        id: null,
        title: '',
        currentAssignment: null, // 解决ID
        currentAssignmentName: '', // 解决人
        comment: '', // 备注
      },
      rules: {
        title: [{ required: true, message: '请输入问题简述', trigger: 'blur' }],
        currentAssignmentName: [{ required: true, message: '请选择解决人', trigger: 'change' }],
      },
      visible: false,
      isShow: true,
    }
  },
  computed: {
    userSelect() {
      return {
        id: this.form.currentAssignment,
        name: this.form.currentAssignmentName,
      }
    },
  },
  methods: {
    initRedispatch(record, isShow = true) {
      this.visible = true
      this.form.id = record.id
      this.form.title = record.title
      this.isShow = isShow
    },

    // 模糊搜索选中人员
    handlerSelectUser(valueObj) {
      this.form.currentAssignment = valueObj.value
      this.form.currentAssignmentName = valueObj.label
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
          const { currentAssignmentName, ...parameter } = this.form
          const parameterArray = [parameter]
          IssueRedispatch(parameterArray)
            .then((res) => {
              if (res.success) {
                this.$message.success('问题转交成功')
                this.getProblemList()
                this.handleCancel()
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
      Object.assign(this, {
        form: {
          id: null,
          title: '',
          currentAssignment: null, // 解决ID
          currentAssignmentName: '', // 解决人
          comment: '', // 备注
        },
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