<!--
 * @Author: 林伟群
 * @Date: 2022-05-30 09:56:36
 * @LastEditTime: 2022-05-31 17:26:49
 * @LastEditors: 林伟群
 * @Description: 解决问题
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\ProblemSolve.vue
-->
<template>
  <a-modal v-model="visible" :title="versionTitle" on-ok="handleOk">
    <section v-show="!isVersion" class="form_1">
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
        <a-form-model-item label="实际完成时间" prop="solveTime">
          <a-date-picker
            style="width: 100%"
            placeholder="请选择预计完成时间"
            v-model="form.solveTime"
            @change="attributDate"
            @focus="attributDateType('solveTime')"
          />
        </a-form-model-item>
        <a-form-model-item label="解决版本" prop="solveVersion">
          <section class="from_chilen">
            <a-select v-model="form.solveVersion" placeholder="请选择问题分类">
              <a-select-option v-for="(item, index) in versionArray" :key="index" :value="item.name">{{
                item.name
              }}</a-select-option>
            </a-select>
            <a-button @click="creaVersion"> 创建版本 </a-button>
          </section>
        </a-form-model-item>
        <a-form-model-item ref="reason" label="原因分析" prop="reason">
          <a-textarea
            v-model="form.reason"
            @blur="
              () => {
                $refs.reason.onFieldBlur()
              }
            "
            :rows="4"
            placeholder="请输入原因分析"
          />
        </a-form-model-item>
        <a-form-model-item ref="measures" label="解决措施" prop="measures">
          <a-textarea
            v-model="form.measures"
            @blur="
              () => {
                $refs.measures.onFieldBlur()
              }
            "
            :rows="4"
            placeholder="请输入解决措施"
          />
        </a-form-model-item>

        <a-form-item label="附件上传"> <ProblemUplod @uploadProblem="uploadProblem"></ProblemUplod> </a-form-item>
      </a-form-model>
      <OperRecords :id="form.id" isModal v-if="isShow"></OperRecords>
    </section>
    <section v-show="isVersion" class="form_1">
      <a-form-model
        ref="versionForm"
        :labelCol="labelCol"
        :wrapperCol="wrapperCol"
        :model="versionForm"
        :rules="versionFormRules"
      >
        <a-form-model-item label="版本类别" prop="type">
          <a-select v-model="versionForm.type" style="width: 100%" placeholder="请选择版本类别">
            <a-select-option v-for="(item, index) in versionTypeArray" :key="index" :value="item.type">{{
              item.type
            }}</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item ref="version" label="版本号" prop="version">
          <a-input
            v-model="versionForm.version"
            @blur="
              () => {
                $refs.version.onFieldBlur()
              }
            "
            placeholder="请输入版本号"
          />
        </a-form-model-item>
      </a-form-model>
    </section>
    <template slot="footer">
      <section v-if="!isVersion">
        <a-button @click="handleSubmit" type="primary"> 提交 </a-button>
        <a-button @click="handleCancel"> 返回 </a-button>
      </section>
      <section v-else>
        <a-button @click="preserveVersion" type="primary"> 保存 </a-button>
      </section>
    </template>
  </a-modal>
</template>

<script>
import { IssueAttachmentSaveId, IssueExecute } from '@/api/modular/main/SsuIssueManage'
import { getalltypelist, addversion } from '@/api/modular/system/versionManage'
import OperRecords from './OperRecords.vue'
import ProblemUplod from './ProblemUplod.vue'
export default {
  components: {
    OperRecords,
    ProblemUplod,
  },
  data() {
    return {
      labelCol: { md: { span: 24 }, lg: { span: 5 } },
      wrapperCol: { md: { span: 24 }, lg: { span: 18 } },
      labelCol2: { md: { span: 6 }, lg: { span: 6 }, xs: { span: 8 } },
      visible: false,
      form: {
        id: null,
        title: '', // 问题简述，
        solveTime: undefined, // 预计完成时间
        reason: '', // 原因分析
        measures: '', // 解决措施
        solveVersion: '', // 解决版本
      },
      rules: {
        title: [{ required: true, message: '请输入问题简述', trigger: 'blur' }],
        reason: [{ required: true, message: '请输入原因分析', trigger: 'blur' }],
        measures: [{ required: true, message: '请输入解决措施', trigger: 'blur' }],
      },
      dateType: '',
      isVersion: false,
      versionForm: {
        type: '',
        version: '',
      },
      versionFormRules: {
        version: [{ required: true, message: '请输入问题简述', trigger: 'blur' }],
        type: [{ required: true, message: '请选择版本类别', trigger: 'change' }],
      },
      versionArray: [],
      versionTypeArray: [],
      attachment: {}, // 附件上传的数据
      isShow: true,
    }
  },
  watch: {
    isVersion: {
      handler() {
        this.getVersionList()
      },
      immediate: true,
    },
  },
  inject: ['getProblemList'],
  computed: {
    versionTitle() {
      return this.isVersion ? '创建版本' : '解决问题'
    },
  },
  methods: {
    initSolv(record, isShow = true) {
      this.visible = true
      this.form.id = record.id
      this.form.title = record.title // 问题简述，
      this.isShow = isShow
    },
    // 动态属性日期类型
    attributDateType(fieldCode) {
      this.dateType = fieldCode
    },
    // 动态属性日期
    attributDate(dates, dateStrings) {
      if (this.dateType == 'solveTime') {
        this.form[this.dateType] = dateStrings
      }
    },

    // 获取版本列表
    getVersionList() {
      getalltypelist()
        .then((res) => {
          if (res.success) {
            this.versionArray = res.data.map((item) => {
              item.name = item.type + ' ' + item.version
              return item
            })
            const versionType = res.data.map((item) => {
              return JSON.stringify({ type: item.type })
            })
            const versionTypeList = [...new Set(versionType)]
            this.versionTypeArray = versionTypeList.map((item) => {
              return JSON.parse(item)
            })
            console.log(this.versionTypeArray)
          } else {
            this.$message.warning(res.message)
          }
        })
        .catch(() => {
          this.$message.error('版本列表获取失败')
        })
    },
    // 创建版本
    creaVersion() {
      this.isVersion = true
    },
    // 保存版本
    preserveVersion() {
      this.$refs.versionForm.validate((valid) => {
        if (valid) {
          addversion(this.versionForm)
            .then((res) => {
              if (res.success) {
                this.$message.success('版本创建成功')
              } else {
                this.$message.warning(res.message)
              }
              this.isVersion = false
            })
            .catch(() => {
              this.$message.error('版本创建失败')
              this.isVersion = false
            })
        } else {
          return false
        }
      })
    },

    // 附件上传
    uploadProblem(val) {
      this.attachment = val
    },

    // 提交
    handleSubmit() {
      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          IssueExecute(this.form)
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
                this.$message.success('问题处理成功')
                this.visible = false
                this.getProblemList()
              } else {
                this.$message.warning(res.message)
              }
            })
            .catch(() => {
              this.$message.error('问题处理成功')
            })
        } else {
          return false
        }
      })
    },
    // 返回
    handleCancel() {
      this.visible = !this.visible
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



