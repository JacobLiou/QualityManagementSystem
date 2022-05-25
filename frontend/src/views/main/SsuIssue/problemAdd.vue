<!--
 * @Author: 林伟群
 * @Date: 2022-05-19 10:30:06
 * @LastEditTime: 2022-05-24 15:37:21
 * @LastEditors: 林伟群
 * @Description: 问题增加页面
 * @FilePath: \frontend\src\views\main\SsuIssue\problemAdd.vue
-->
<template>
  <a-card class="add">
    <div class="add_title">问题新增</div>
    <section class="form_1">
      <a-form-model ref="ruleForm" :labelCol="labelCol" :wrapperCol="wrapperCol" :model="form" :rules="rules">
        <a-form-model-item label="所属项目" prop="projectId">
          <a-select v-model="form.projectId" placeholder="请选择所属项目">
            <a-select-option v-for="(item, index) in projectData" :key="index" :value="item.id">{{
              item.projectName
            }}</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="产品" prop="productId">
          <a-select v-model="form.productId" placeholder="请选择产品">
            <a-select-option v-for="(item, index) in productData" :key="index" :value="item.id">{{
              item.productName
            }}</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="模块" prop="module">
          <a-select v-model="form.module" placeholder="请选择模块" @change="moduleChange">
            <a-select-option v-for="item in moduleData" :key="item.code" :value="Number(item.code)">{{
              item.name
            }}</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="问题分类" prop="issueClassification">
          <a-select v-model="form.issueClassification" placeholder="请选择问题分类">
            <a-select-option v-for="(item, index) in issueClassificationData" :key="index" :value="Number(item.code)">{{
              item.name
            }}</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item ref="dispatcherName" label="当前指派" prop="dispatcherName">
          <section class="from_chilen">
            <a-input
              v-model="form.dispatcherName"
              @blur="
                () => {
                  $refs.dispatcherName.onFieldBlur()
                }
              "
              placeholder="请选择指派人"
              disabled
            />
            <a-button @click="changePersonnel('dispatcher')" :disabled="isEdit"> 选择 </a-button>
          </section>
        </a-form-model-item>
        <a-form-model-item label="性质" prop="consequence">
          <a-select v-model="form.consequence" placeholder="请选择问题性质">
            <a-select-option v-for="(item, index) in consequenceData" :key="index" :value="Number(item.code)">{{
              item.name
            }}</a-select-option>
          </a-select>
        </a-form-model-item>
        <section class="add_once">
          <a-form-model-item ref="title" label="问题简述" prop="title" :labelCol="labelCol2" :wrapperCol="wrapperCol2">
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
        </section>
        <section class="add_once">
          <a-form-model-item label="问题描述" prop="description" :labelCol="labelCol2" :wrapperCol="wrapperCol2">
            <VueQuillEditor v-model="form.description"></VueQuillEditor>
          </a-form-model-item>
        </section>
        <a-form-model-item label="问题来源" prop="source">
          <a-select v-model="form.source" placeholder="请选择问题来源">
            <a-select-option v-for="(item, index) in sourceData" :key="index" :value="Number(item.code)">{{
              item.name
            }}</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="发现人" prop="discoverName">
          <section class="from_chilen">
            <a-input v-model="form.discoverName" placeholder="请选择发现人" disabled />
            <a-button @click="changePersonnel('discover')"> 选择 </a-button>
          </section>
        </a-form-model-item>
        <a-form-model-item label="发现日期" prop="discoverTime">
          <a-date-picker
            style="width: 100%"
            placeholder="请选择发现日期"
            v-model="form.discoverTime"
            @change="attributDate"
            @focus="attributDateType('discoverTime')"
          />
        </a-form-model-item>
        <a-form-model-item label="抽送" prop="ccListNmae">
          <section class="from_chilen">
            <a-input v-model="form.ccListNmae" placeholder="请选择抽送人" disabled />
            <a-button @click="changePersonnel('ccList')" :disabled="isEdit"> 选择 </a-button>
          </section>
        </a-form-model-item>
        <!-- 新增的属性 -->
        <!-- <section class="add_once">
          <a-form-model ref="attribuForm" :labelCol="labelCol" :wrapperCol="wrapperCol" :model="attribuForm"> -->
        <section class="add_two">
          <a-form-model-item :label="attItem.fieldName" v-for="attItem in extendAttributeList" :key="attItem.fieldCode">
            <!-- input输入框  -->
            <section class="from_chilen" v-if="attItem.fieldDataType == 'string'">
              <a-input v-model="attribuForm[attItem.fieldCode]" :placeholder="attItem | placeholderName" />
            </section>
            <!-- 单选控件 -->
            <section class="from_chilen" v-if="attItem.fieldDataType == 'bool'">
              <a-radio-group
                v-model="attribuForm[attItem.fieldCode]"
                v-for="item in checkAttArray(attItem.fieldCode, true)"
                :key="item.label"
              >
                <a-radio :value="item.value"> {{ item.label }} </a-radio>
              </a-radio-group>
            </section>
            <!-- 日期选择控件 -->
            <section class="from_chilen" v-if="attItem.fieldDataType == 'DateTime'">
              <a-date-picker
                style="width: 100%"
                :placeholder="attItem | placeholderName"
                format="YYYY-MM-DD"
                :v-model="attribuForm[attItem.fieldCode]"
                @change="attributDate"
                @focus="attributDateType(attItem.fieldCode)"
              />
            </section>
            <!-- 小数输入框 -->
            <section class="from_chilen" v-if="attItem.fieldDataType == 'decimal'">
              <a-input-number v-model="attribuForm[attItem.fieldCode]" :min="0" :step="0.1" />
            </section>
            <!-- 复选框 -->
            <section class="from_chilen" v-if="attItem.fieldDataType == 'enum' && attItem.fieldName == '样机说明'">
              <a-checkbox-group v-model="attribuForm[attItem.fieldCode]">
                <a-row style="width: 100%" :gutter="[2, 2]">
                  <a-col :span="8" v-for="(item, index) in checkAttArray(attItem.fieldCode, true)" :key="index">
                    <a-checkbox :value="item.value">
                      {{ item.label }}
                    </a-checkbox>
                  </a-col>
                </a-row>
              </a-checkbox-group>
            </section>
            <!-- 下拉列表 -->
            <section class="from_chilen" v-if="attItem.fieldDataType == 'enum' && attItem.fieldName !== '样机说明'">
              <a-select
                style="width: 100%"
                v-model="attribuForm[attItem.fieldCode]"
                :placeholder="attItem | placeholderName"
              >
                <a-select-option
                  v-for="item in checkAttArray(attItem.fieldCode)"
                  :key="item.code"
                  :value="Number(item.code)"
                  >{{ item.name }}</a-select-option
                >
              </a-select>
            </section>
            <!-- 整数输入框 -->
            <section class="from_chilen" v-if="attItem.fieldDataType == 'int'">
              <a-input-number v-model="attribuForm[attItem.fieldCode]" :min="0" />
            </section>
            <!-- 人员选择控件 -->
            <section class="from_chilen" v-if="attItem.fieldDataType == 'long'">
              <!-- TODO 逻辑待定 -->
              <a-input v-model="attribuForm[attItem.fieldCode]" :placeholder="attItem | placeholderName" disabled />
              <a-button @click="changePersonnel(attItem.fieldCode)"> 选择 </a-button>
            </section>
          </a-form-model-item>
        </section>
        <!-- </a-form-model>
        </section> -->
        <section class="add_once">
          <a-form-item label="附件上传" :labelCol="labelCol2">
            <a-upload :customRequest="customRequest" :multiple="true" :showUploadList="true" name="file">
              <a-button icon="upload">附件上传</a-button>
            </a-upload>
          </a-form-item>
        </section>
        <section class="add_once">
          <a-form-model-item :wrapper-col="wrapperCol3">
            <a-button type="primary" @click="onSubmit"> 添加 </a-button>
            <a-button style="margin-left: 10px" @click="resetForm"> 重置 </a-button>
            <a-button style="margin-left: 10px" type="primary" @click="onStorage"> 暂存 </a-button>
            <a-button style="margin-left: 10px" @click="addAttribute"> 添加属性 </a-button>
            <a-button style="margin-left: 10px" @click="onback"> 返回 </a-button>
          </a-form-model-item>
        </section>
      </a-form-model>
    </section>
    <!-- 选择人员 -->
    <CheckUserList
      :userVisible="userVisible"
      :personnelType="personnelType"
      @checkUserArray="checkUserArray"
    ></CheckUserList>
    <!-- 选择属性 -->
    <AttributCheck
      :attributVisible="attributVisible"
      :moduleType="form.module"
      @handleAttribut="handleAttribut"
    ></AttributCheck>
  </a-card>
</template>

<script>
import { SsuProjectList } from '@/api/modular/main/SsuProjectManage'
import { SsuProductList } from '@/api/modular/main/SsuProductManage'
import { IssueAdd, IssueAttachmentSaveId, IssueDetail } from '@/api/modular/main/SsuIssueManage'
import VueQuillEditor from './componets/VueQuillEditor.vue'
import CheckUserList from './componets/CheckUserList.vue'
import AttributCheck from './componets/AttributCheck.vue'
import { sysFileInfoUpload } from '@/api/modular/system/fileManage'

export default {
  components: { CheckUserList, VueQuillEditor, AttributCheck },
  data() {
    return {
      labelCol: { md: { span: 24 }, lg: { span: 6 } },
      wrapperCol: { md: { span: 24 }, lg: { span: 10 } },
      labelCol2: { md: { span: 24 }, lg: { span: 3 } },
      wrapperCol2: { md: { span: 24 }, lg: { span: 17 } },
      wrapperCol3: { md: { span: 24 }, lg: { span: 17, offset: 3 } },
      other: '',
      form: {
        title: '', // 问题简述，
        projectId: undefined, // 项目编号
        productId: undefined, // 产品编号
        module: undefined, // 模块
        issueClassification: undefined, // 问题分类
        dispatcher: undefined, // 分发人ID（指派人）
        dispatcherName: '', // 分发人名字
        consequence: undefined, // 性质
        // 下边不是必传字段
        source: null, // 问题来源
        discover: '', // 发现人
        discoverName: '', // 发现人名字
        discoverTime: null, // 发现日期
        ccList: [], // 抽送人
        ccListNmae: '', // 抽送人名字
        description: '', // 详情
        extendAttribute: '', // 新增字段
      },
      rules: {
        title: [{ required: true, message: '请输入问题简述', trigger: 'blur' }],
        dispatcherName: [{ required: true, message: '请选择指派人', trigger: 'changes' }],
        projectId: [{ required: true, message: '请选择所属项目', trigger: 'change' }],
        productId: [{ required: true, message: '请选择产品编号', trigger: 'change' }],
        module: [{ required: true, message: '请选择模块', trigger: 'change' }],
        issueClassification: [{ required: true, message: '请选择问题分类', trigger: 'change' }],
        consequence: [{ required: true, message: '请选择性质', trigger: 'change' }],
      },
      projectData: [], // 项目list
      productData: [], // 产品list
      moduleData: [], // 模块list
      issueClassificationData: [], // 问题分类list
      consequenceData: [], // 问题性质
      sourceData: [], // 问题来源
      ccListData: [], // 抽送人数组
      discoverData: [], // 发现人数组
      dispatcherData: [], // 分发人数组
      userVisible: false, // 抽屉显示
      personnelType: '', // 选择的人
      attributVisible: false, // 选择属性
      extendAttributeList: [], // 新增的属性
      attribuForm: {}, // 新增属性表单
      moduleType: '', // 新增类型属性
      dateType: '', // 时间类型
      attachment: {}, // 附件参数
      isEdit: false, // 问题编辑
    }
  },
  created() {
    this.initList()
    this.getFromData()
    const { storageAddform, editProblem, checkRecord } = this.$store.state.record
    if (storageAddform.isStorage) {
      this.form = storageAddform.form
      this.extendAttributeList = storageAddform.extendAttributeList
      this.attribuForm = storageAddform.attribuForm
      this.attachment = storageAddform.attachment
    }
    if (editProblem.isEdit) {
      this.isEdit = editProblem.isEdit
      const id = this.$route.query.editId
      this.form = checkRecord
      this.getIssueDetail(id)
      console.log('编辑的数据', checkRecord)
    }
  },
  filters: {
    placeholderName(item) {
      let constent = '请选择'
      switch (item.fieldDataType) {
        case 'DateTime':
          constent = constent + item.fieldName
          break
        case 'enum':
          constent = constent + item.fieldName
          break
        default:
          constent = '请输入' + item.fieldName
          break
      }
      return constent
    },
  },
  methods: {
    // 获取详情
    getIssueDetail(id) {
      IssueDetail({ id })
        .then((res) => {
          console.log(res)
          if (res.success) {
            console.log(res)
            // this.form.description = res.data.description
            this.extendAttributeList = JSON.parse(res.data.extendAttribute)
            // this.IssueDetailData = res.data
            console.log(res.data.extendAttribute)
            // this.extendAttribute = this.changeExtendAttribute(res.data.extendAttribute)
            console.log(JSON.parse(res.data.extendAttribute))
          }
        })
        .catch((err) => {
          this.$message.error('问题详情查看失败')
        })
    },
    // 列表初始化
    initList() {
      this.moduleData = this.$options.filters['dictData']('issue_module')
      console.log('this.moduleData', this.moduleData)
      this.issueClassificationData = this.$options.filters['dictData']('issue_classification')
      this.consequenceData = this.$options.filters['dictData']('issue_consequence')
      this.sourceData = this.$options.filters['dictData']('issue_source')
    },
    // 获取相应的字段
    getFromData() {
      // 项目
      SsuProjectList()
        .then((res) => {
          if (res.success) {
            this.projectData = res.data
          } else {
            this.$message.error('项目列表读取失败')
          }
        })
        .finally((res) => {
          this.confirmLoading = false
        })
      // 产品
      SsuProductList()
        .then((res) => {
          if (res.success) {
            this.productData = res.data
          } else {
            this.$message.error('产品列表读取失败')
          }
        })
        .finally((res) => {
          this.confirmLoading = false
        })
    },

    //模块选择
    moduleChange(value) {
      if (this.moduleType !== value) {
        this.moduleType = value
        this.attribuForm = {}
        this.extendAttributeList = []
      }
    },

    // 动态属性选择按钮操作
    checkAttArray(fieldCode, check = false) {
      const attArray = this.$options.filters['dictData'](fieldCode)
      if (!check) return attArray
      const newAttArray = attArray.map((item) => {
        return { label: item.name, value: item.code }
      })
      return newAttArray
    },
    // 动态属性日期类型
    attributDateType(fieldCode) {
      this.dateType = fieldCode
    },
    // 动态属性日期
    attributDate(dates, dateStrings) {
      console.log(this.dateType)
      if (this.dateType == 'discoverTime') {
        this.form[this.dateType] = dateStrings
        console.log(this.form)
      } else {
        this.attribuForm[this.dateType] = dateStrings
      }
    },
    // 选择人员
    changePersonnel(value) {
      this.personnelType = value
      this.userVisible = !this.userVisible
    },

    // 获取选择的人员
    checkUserArray(checkUser) {
      if (checkUser.length === 0) return
      let perArray = []
      switch (this.personnelType) {
        case 'dispatcher': // 分发指派人
          this.dispatcherData = checkUser
          perArray = checkUser.map((item) => {
            return item.name
          })
          this.form.dispatcherName = perArray.join()
          this.form.dispatcher = Number(checkUser[0].id)
          break
        case 'discover': // 发现指派人
          this.discoverData = checkUser
          perArray = checkUser.map((item) => {
            return item.name
          })
          this.form.discoverName = perArray.join()
          this.form.discover = Number(checkUser[0].id)
          break
        case 'ccList': // 抽送指派人
          this.ccListData = checkUser
          perArray = checkUser.map((item) => {
            return item.name
          })
          this.form.ccList = checkUser.map((item) => {
            return Number(item.id)
          })
          this.form.ccListNmae = perArray.join()
          break
        default:
          perArray = checkUser.map((item) => {
            return item.name
          })
          const customAttribu = this.personnelType + 'customNameId'
          this.attribuForm[customAttribu] = Number(checkUser[0].id)
          this.attribuForm[this.personnelType] = perArray.join()
          this.$forceUpdate()
          console.log(this.attribuFormChange())
          break
      }
    },

    // 附件上传
    customRequest(data) {
      const { file } = data
      const formData = new FormData()
      formData.append('file', file)
      sysFileInfoUpload(formData).then((res) => {
        if (res.success) {
          this.$message.success('附件上传成功')
          this.attachment = {
            attachmentId: res.data,
            fileName: file.name,
            attachmentType: 0,
          }
        } else {
          this.$message.error('上传失败：' + res.message)
        }
      })
    },
    // 提交
    onSubmit() {
      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          this.form.extendAttribute = this.attribuFormChange()
          IssueAdd(this.form)
            .then((res) => {
              if (res.success) {
                // const issueId = res.data.id
                // const parameter = {
                //   attachment: this.attachment,
                //   issueId: issueId,
                // }
                this.$message.success('问题增加成功')
                // IssueAttachmentSaveId(parameter)
                //   .then((res) => {
                //     if (!res.success) {
                //       this.$message.error('附件信息保存失败：' + res.message)
                //     }
                //   })
                //   .catch(() => {
                //     this.$message.error('附件信息保存失败：' + res.message)
                //   })
                this.$router.replace({
                  path: '/problemManagement',
                })
                this.$store.commit('SET_ADD_FORM', {})
              } else {
                this.$message.error('问题增加失败')
              }
            })
            .catch(() => {
              this.$message.error('问题增加失败')
            })
        } else {
          console.log(this.form)
          console.log('error submit!!')
          return false
        }
      })
    },

    // 新增表单数据改造
    attribuFormChange() {
      if (this.extendAttributeList.length === 0) {
        return ''
      }
      const newEAL = this.extendAttributeList.map((item) => {
        const fieldCode = item.fieldCode
        const fieldDataType = item.fieldDataType
        const fieldName = item.fieldName
        if (fieldDataType == 'long') {
          item.value = this.attribuForm[fieldCode + 'customNameId']
        } else {
          if (fieldName === '样机说明') {
            // item.value = JSON.stringify(this.attribuForm[fieldCode])
            item.value = this.attribuForm[fieldCode].join()
            console.log(item.value)
          } else {
            item.value = this.attribuForm[fieldCode]
          }
        }
        item.issueId = item.issueId ?? 0
        return item
      })
      console.log('newEAL', newEAL)
      return JSON.stringify(newEAL)
    },

    // 重置
    resetForm() {
      this.$refs.ruleForm.resetFields()
    },
    // 暂存
    onStorage() {
      const storageAddform = {
        form: this.form,
        extendAttributeList: this.extendAttributeList,
        attribuForm: this.attribuForm,
        attachment: this.attachment,
        isStorage: true,
      }
      this.$store.commit('SET_ADD_FORM', storageAddform)
      this.$message.success('问题已暂存')
    },
    // 添加属性
    addAttribute() {
      if (this.form.module == undefined) {
        this.$message.warning('请选择模块')
        return
      }
      this.attributVisible = !this.attributVisible
    },
    handleAttribut(val) {
      this.extendAttributeList = val.map((item) => JSON.parse(item))
      console.log('新增属性', this.extendAttributeList)
    },
    // 返回
    onback() {
      this.$router.back()
    },
  },
}
</script>

<style lang="less" scoped>
.add {
  .add_title {
    font-size: 1.2em;
    font-weight: 700;
  }
  .form_1 {
    /deep/.ant-form {
      display: flex;
      flex-wrap: wrap;
    }
    /deep/.ant-row {
      width: 50%;
    }
    .from_chilen {
      display: flex;
    }
    .add_once {
      width: 100%;
      /deep/.ant-row {
        width: 100%;
      }
    }
    .add_two {
      width: 100%;
      display: flex;
      flex-wrap: wrap;
      /deep/.ant-row {
        width: 50%;
      }
    }

    @media screen and (max-width: 992px) {
      /deep/.ant-form {
        display: unset;
      }
      /deep/.ant-row {
        width: 100%;
      }
      .add_two {
        display: unset;
        /deep/.ant-row {
          width: 100%;
        }
      }
    }
  }
}
</style>