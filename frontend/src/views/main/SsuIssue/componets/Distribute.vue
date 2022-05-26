<!--
 * @Author: 林伟群
 * @Date: 2022-05-26 10:19:12
 * @LastEditTime: 2022-05-26 14:25:51
 * @LastEditors: 林伟群
 * @Description: 问题分派
 * @FilePath: \frontend\src\views\main\SsuIssue\componets\Distribute.vue
-->
<template>
  <a-modal v-model="visible" title="问题分发" on-ok="handleOk">
    <section class="form_1">
      <a-form-model ref="ruleForm" :labelCol="labelCol2" :wrapperCol="wrapperCol2" :model="form" :rules="rules">
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
        <a-form-model-item label="问题分类" prop="issueClassification">
          <a-select v-model="form.issueClassification" placeholder="请选择问题分类">
            <a-select-option
              v-for="(item, index) in checkAttArray('issue_classification')"
              :key="index"
              :value="Number(item.code)"
              >{{ item.name }}</a-select-option
            >
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="性质" prop="consequence">
          <a-select v-model="form.consequence" placeholder="请选择问题性质">
            <a-select-option
              v-for="(item, index) in checkAttArray('issue_consequence')"
              :key="index"
              :value="Number(item.code)"
              >{{ item.name }}</a-select-option
            >
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="责任人" prop="executorName">
          <section class="from_chilen">
            <a-input v-model="form.executorName" placeholder="请选择责任人" disabled />
            <a-button @click="changePersonnel('executor')"> 选择 </a-button>
          </section>
        </a-form-model-item>
        <a-form-model-item label="预计完成时间" prop="forecastSolveTime">
          <a-date-picker
            style="width: 100%"
            placeholder="请选择预计完成时间"
            v-model="form.discoverTime"
            @change="attributDate"
            @focus="attributDateType('forecastSolveTime')"
          />
        </a-form-model-item>
        <a-form-model-item label="抽送" prop="ccListNmae">
          <section class="from_chilen">
            <a-input v-model="form.ccListNmae" placeholder="请选择抽送人" disabled />
            <a-button @click="changePersonnel('ccList')"> 选择 </a-button>
          </section>
        </a-form-model-item>
        <!-- 动态属性添加 todo -->
        <!-- <section class="add_once"> -->
        <a-form-item label="附件上传" :labelCol="labelCol2">
          <a-upload :customRequest="customRequest" :multiple="true" :showUploadList="true" name="file">
            <a-button icon="upload">附件上传</a-button>
          </a-upload>
        </a-form-item>
        <!-- </section> -->
        <!-- <section class="add_once"> -->
        <a-form-model-item :wrapper-col="wrapperCol3">
          <a-button style="margin-left: 10px" @click="addAttribute"> 添加属性 </a-button>
        </a-form-model-item>
        <!-- </section> -->
      </a-form-model>
    </section>
    <AttributCheck
      :attributVisible="attributVisible"
      :moduleType="module"
      @handleAttribut="handleAttribut"
    ></AttributCheck>
  </a-modal>
</template>

<script>
// import CheckUserList from './CheckUserList.vue'
import AttributCheck from './CheckUserList.vue'
export default {
  components: { AttributCheck },
  props: {
    distributeVisible: {
      type: Boolean,
      default: Boolean,
    },
    distributeRecord: {
      type: [Object],
    },
  },
  data() {
    return {
      labelCol: { md: { span: 24 }, lg: { span: 6 } },
      wrapperCol: { md: { span: 24 }, lg: { span: 10 } },
      labelCol2: { md: { span: 6 }, lg: { span: 6 }, xs: { span: 8 } },
      wrapperCol2: { md: { span: 16 }, lg: { span: 16 }, xs: { span: 16 } },
      wrapperCol3: { md: { span: 24 }, lg: { span: 17, offset: 3 } },
      visible: false,
      form: {
        id: null,
        title: '', // 问题简述，
        forecastSolveTime: undefined, // 预计完成时间
        issueClassification: undefined, // 问题分类
        consequence: undefined, // 性质
        executor: null, // 执行人id
        executorName: '', // 执行人名字
        // 下边不是必传字段
        ccList: [], // 抽送人
        ccListNmae: '', // 抽送人名字
        extendAttribute: '', // 新增字段
      },
      rules: {
        title: [{ required: true, message: '请输入问题简述', trigger: 'blur' }],
        executorName: [{ required: true, message: '请选择执行人', trigger: 'changes' }],
        forecastSolveTime: [{ required: true, message: '请选择预计完成时间', trigger: 'change' }],
        issueClassification: [{ required: true, message: '请选择问题分类', trigger: 'change' }],
        consequence: [{ required: true, message: '请选择性质', trigger: 'change' }],
      },
      module: undefined, // 动态属性添加
      attributVisible: false, // 显示动态属性弹窗
      extendAttributeList: [], // 新增的动态属性
    }
  },
  watch: {
    distributeVisible() {
      this.visible = !this.visible
      const value = this.distributeRecord
      console.log(value)
      this.form.id = value.id
      this.form.title = value.title // 问题简述，
      this.form.forecastSolveTime = value.forecastSolveTime // 预计完成时间
      this.form.issueClassification = value.issueClassification // 问题分类
      this.form.consequence = value.consequence // 性质
      this.form.executor = value.executor // 执行人id
      this.form.executorName = value.executorName // 执行人名字
      this.form.ccList = value.copyTo // 抽送人
      this.form.ccListNmae = value.copyToName?.join() // 抽送人名字
      this.module = value.module
    },
  },
  methods: {
    // 选择属性
    checkAttArray(fieldCode, check = false) {
      const attArray = this.$options.filters['dictData'](fieldCode)
      if (!check) return attArray
      const newAttArray = attArray.map((item) => {
        return { label: item.name, value: item.code }
      })
      return newAttArray
    },
    // 附件上传
    customRequest() {},
    attributDate() {},
    attributDateType() {},
    changePersonnel() {},
    // 动态属性添加
    addAttribute() {
      this.attributVisible = !this.attributVisible
    },
    handleAttribut(val) {
      this.extendAttributeList = val.map((item) => JSON.parse(item))
      console.log('新增属性', this.extendAttributeList)
    },
  },
}
</script>

<style lang="less" scoped>
.form_1 {
  /deep/.ant-row {
    display: flex;
    align-items: center;
  }
  .from_chilen {
    display: flex;
  }
  //   .add_once {
  //     width: 100%;
  //     /deep/.ant-row {
  //       width: 100%;
  //     }
}
//   .add_two {
//     width: 100%;
//     display: flex;
//     flex-wrap: wrap;
//     /deep/.ant-row {
//       width: 50%;
//     }
//   }

//   @media screen and (max-width: 992px) {
//     /deep/.ant-form {
//       display: unset;
//     }
//     /deep/.ant-row {
//       width: 100%;
//     }
//     .add_two {
//       display: unset;
//       /deep/.ant-row {
//         width: 100%;
//       }
//     }
//   }
// }
</style>