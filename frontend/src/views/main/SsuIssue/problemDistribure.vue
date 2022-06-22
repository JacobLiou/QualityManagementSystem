<!--
 * @Author: 林伟群
 * @Date: 2022-05-26 14:29:27
 * @LastEditTime: 2022-06-21 15:47:38
 * @LastEditors: 林伟群
 * @Description: 问题分发页面
 * @FilePath: \frontend\src\views\main\SsuIssue\problemDistribure.vue
-->
<template>
  <a-card class="add">
    <div class="add_title">问题分发</div>
    <section class="form_1">
      <a-form-model ref="ruleForm" :labelCol="labelCol" :wrapperCol="wrapperCol" :model="form" :rules="rules">
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
          <a-select v-model="form.consequence" placeholder="请选择问题性质" @change="handleConsequence">
            <a-select-option
              v-for="(item, index) in checkAttArray('issue_consequence')"
              :key="index"
              :value="Number(item.code)"
              >{{ item.name }}</a-select-option
            >
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="责任人" prop="currentAssignmentName">
          <section class="from_chilen">
            <!-- <a-input v-model="form.currentAssignmentName" placeholder="请选择责任人" disabled /> -->
            <SelectUser
              title="请输入责任人"
              @handlerSelectUser="handlerSelectUser"
              selectType="currentAssignment"
              :userSelect="userSelect"
            ></SelectUser>
            <a-button @click="changePersonnel('currentAssignment')"> 选择 </a-button>
          </section>
        </a-form-model-item>
        <a-form-model-item label="预计完成时间" prop="forecastSolveTime">
          <a-date-picker
            style="width: 100%"
            placeholder="请选择预计完成时间"
            v-model="form.forecastSolveTime"
            @change="attributDate"
            @focus="attributDateType('forecastSolveTime')"
          />
        </a-form-model-item>
        <a-form-model-item label="抄送" prop="ccListName">
          <section class="from_chilen">
            <!-- <a-input v-model="form.ccListName" placeholder="请选择抄送人" disabled :title="form.ccListName" /> -->
            <SelectUserMore
              title="请输入抄送人"
              @handlerSelectUser="handlerSelectUser"
              selectType="ccList"
              :userSelect="userCcList"
            ></SelectUserMore>
            <a-button @click="changePersonnel('ccList')"> 选择 </a-button>
          </section>
        </a-form-model-item>
        <!-- 新增的属性 -->
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
                @change="attribuCheck"
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
                v-model="attribuForm[attItem.fieldCode]"
                @change="attributDate"
                @focus="attributDateType(attItem.fieldCode)"
              />
            </section>
            <!-- 小数输入框 -->
            <section class="from_chilen" v-if="attItem.fieldDataType == 'decimal'">
              <a-input-number
                v-model="attribuForm[attItem.fieldCode]"
                :min="0"
                :step="0.1"
                :disabled="attItem.fieldCode == 'ImpactScore'"
              />
            </section>
            <!-- 复选框 -->
            <section class="from_chilen" v-if="attItem.fieldDataType == 'enum' && attItem.fieldName == '样机说明'">
              <a-checkbox-group v-model="attribuForm[attItem.fieldCode]" @change="attribuCheck">
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
                @change="attribuCheck"
              >
                <a-select-option v-for="item in checkAttArray(attItem.fieldCode)" :key="item.code" :value="item.code">{{
                  item.name
                }}</a-select-option>
              </a-select>
            </section>
            <!-- 整数输入框 -->
            <section class="from_chilen" v-if="attItem.fieldDataType == 'int'">
              <a-input-number v-model="attribuForm[attItem.fieldCode]" :min="0" />
            </section>
            <!-- 人员选择控件 -->
            <section class="from_chilen" v-if="attItem.fieldDataType == 'long'">
              <!-- TODO 逻辑待定 -->
              <!-- <a-input v-model="attribuForm[attItem.fieldCode]" :placeholder="attItem | placeholderName" disabled /> -->
              <SelectUser
                :title="attItem | placeholderName"
                :selectType="attItem.fieldCode"
                @handlerSelectUser="handlerSelectUser"
                :userSelect="attUser(attItem.fieldCode)"
              ></SelectUser>
              <a-button @click="changePersonnel(attItem.fieldCode)"> 选择 </a-button>
            </section>
          </a-form-model-item>
        </section>
        <section class="add_once">
          <a-form-item label="附件上传" :labelCol="labelCol2">
            <a-upload :customRequest="customRequest" :multiple="true" :showUploadList="true" name="file">
              <a-button icon="upload">附件上传</a-button>
            </a-upload>
          </a-form-item>
        </section>
        <section class="add_once">
          <a-form-model-item :wrapper-col="wrapperCol3">
            <a-button type="primary" @click="onSubmit"> 提交 </a-button>
            <a-button style="margin-left: 10px" @click="onback"> 返回 </a-button>
            <a-button style="margin-left: 10px" @click="addAttribute"> 添加属性 </a-button>
          </a-form-model-item>
        </section>
      </a-form-model>
    </section>
    <!-- 选择属性 -->
    <AttributCheck
      :attributVisible="attributVisible"
      :moduleType="module"
      :initCheckAttr="initCheckAttr"
      @handleAttribut="handleAttribut"
    ></AttributCheck>
    <!-- 选择人员 -->
    <CheckUserList
      :userVisible="userVisible"
      :personnelType="personnelType"
      @checkUserArray="checkUserArray"
    ></CheckUserList>
  </a-card>
</template>

<script>
import CheckUserList from './componets/CheckUserList.vue'
import AttributCheck from './componets/AttributCheck.vue'
import SelectUser from './componets/SelectUser.vue'
import SelectUserMore from './componets/SelectUserMore.vue'
import { IssueDetail, IssueAttachmentSaveId, IssueDispatch } from '@/api/modular/main/SsuIssueManage'
import { sysFileInfoUpload } from '@/api/modular/system/fileManage'
export default {
  components: { CheckUserList, AttributCheck, SelectUser, SelectUserMore },
  data() {
    return {
      labelCol: { md: { span: 24 }, lg: { span: 6 } },
      wrapperCol: { md: { span: 24 }, lg: { span: 10 } },
      labelCol2: { md: { span: 24 }, lg: { span: 3 } },
      wrapperCol2: { md: { span: 24 }, lg: { span: 17 } },
      wrapperCol3: { md: { span: 24 }, lg: { span: 17, offset: 3 } },
      visible: false,
      form: {
        id: null,
        title: '', // 问题简述，
        forecastSolveTime: undefined, // 预计完成时间
        issueClassification: undefined, // 问题分类
        consequence: undefined, // 性质
        currentAssignment: null, // 责任人ID
        currentAssignmentName: '', // 责任人名字
        // 下边不是必传字段
        ccList: [], // 抄送人
        ccListName: '', // 抄送人名字
        extendAttribute: '', // 新增字段
      },
      rules: {
        title: [{ required: true, message: '请输入问题简述', trigger: 'blur' }],
        currentAssignmentName: [{ required: true, message: '请选择责任人', trigger: 'change' }],
        forecastSolveTime: [{ required: true, message: '请选择预计完成时间', trigger: 'change' }],
        issueClassification: [{ required: true, message: '请选择问题分类', trigger: 'change' }],
        consequence: [{ required: true, message: '请选择性质', trigger: 'change' }],
      },
      attribuForm: {},
      module: undefined, // 动态属性添加
      attributVisible: false, // 显示动态属性弹窗
      extendAttributeList: [], // 新增的动态属性
      initCheckAttr: [],
      userVisible: false, // 人员选择显示
      personnelType: '', // 选择的人
      dateType: '', // 时间类型
      attachment: [], // 附件信息
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
  computed: {
    userSelect() {
      return {
        id: this.form.currentAssignment,
        name: this.form.currentAssignmentName,
      }
    },
    attUser() {
      return function (type) {
        return {
          id: this.attribuForm[type + 'customNameId'] ?? type,
          name: this.attribuForm[type],
        }
      }
    },
    userCcList() {
      let { ccListName, ccList } = this.form
      if (ccListName == null && ccList == null) return []
      if (typeof ccListName == 'string') ccListName = ccListName.split(',')
      const selectUserArr = this.form.ccList.map((item, index) => {
        return {
          id: item,
          name: ccListName[index],
        }
      })
      return selectUserArr
    },
  },
  created() {
    const id = this.$route.query.distributeId
    if (id) {
      this.getIssueDetail(id)
    }
  },
  methods: {
    // 获取详情
    getIssueDetail(id) {
      IssueDetail({ id })
        .then((res) => {
          if (res.success) {
            this.initForm(res.data)
            if (!res.data.extendAttribute) return
            const extendAttributeS = JSON.parse(res.data.extendAttribute) || []
            console.log('extendAttributeS', extendAttributeS)
            if (extendAttributeS.length === 0) return
            this.extendAttributeList = extendAttributeS.filter((item) => {
              return item.value !== ''
            })
            this.extendAttributeList.forEach((item) => {
              if (item.fieldName == '样机说明') {
                const arrTrue = item.value.indexOf(',')
                this.attribuForm[item.fieldCode] = arrTrue == -1 ? item.value : item.value.split(',')
              } else {
                this.attribuForm[item.fieldCode] = item.value
              }
            })
            this.initCheckAttr = this.extendAttributeList.map((item) => {
              const { value, issueId, ...newItem } = item
              return JSON.stringify(newItem)
            })
          } else {
            this.$message.warning(res.message)
          }
        })
        .catch(() => {
          this.$message.error('信息获取失败')
        })
    },
    // 初始化form
    initForm(value) {
      // console.log(value, 'value')
      this.form.id = value.id
      this.form.title = value.title // 问题简述，
      this.form.forecastSolveTime = value.forecastSolveTime // 预计完成时间
      this.form.issueClassification = value.issueClassification // 问题分类
      this.form.consequence = value.consequence // 性质
      this.form.currentAssignmentName = value.currentAssignmentName // 执行人idF   要改为指派人
      this.form.currentAssignment = value.currentAssignment // 执行人名字
      this.form.ccList = JSON.parse(value.ccList) // 抄送人
      this.form.ccListName = JSON.parse(value.ccListName)?.join() // 抄送人名字
      this.module = value.module
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

    // 动态属性渲染
    attribuCheck() {
      this.$forceUpdate()
    },

    // 模糊搜索选中人员
    handlerSelectUser(valueObj) {
      console.log('valueObj', valueObj)
      const { selectType } = valueObj
      let perArray = []
      switch (selectType) {
        case 'currentAssignment': // 分发指派人
          this.form.currentAssignment = valueObj.value
          this.form.currentAssignmentName = valueObj.label
          break
        case 'ccList': // 抄送
          const { value } = valueObj
          console.log(value)
          perArray = value.map((item) => {
            return item?.label
          })
          const ccList = value.map((item) => {
            return Number(item?.key)
          })
          const newCcList = [...new Set(ccList)]
          this.form.ccList = newCcList
          const newPerArray = [...new Set(perArray)]
          this.form.ccListName = newPerArray.join()
          console.log(this.form)
          break
        default:
          const customAttribu = selectType + 'customNameId'
          this.attribuForm[customAttribu] = valueObj.value // 存储人员id
          this.attribuForm[selectType] = valueObj.label
          this.$forceUpdate()
          break
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
        case 'currentAssignment': // 分发指派人
          perArray = checkUser.map((item) => {
            return item.name
          })
          this.form.currentAssignmentName = perArray.join()
          this.form.currentAssignment = Number(checkUser[0].id)
          break
        case 'ccList': // 抄送指派人
          perArray = checkUser.map((item) => {
            return item.name
          })
          if (this.form.ccList?.length > 0) {
            const newList = checkUser.map((item) => {
              return Number(item.id)
            })
            const newNameS = perArray
            const oldNameS = this.form.ccListName.split(',')
            const allNameS = [...new Set([...oldNameS, ...newNameS])]
            this.form.ccList = [...new Set([...this.form.ccList, ...newList])]
            console.log(allNameS, this.form.ccList)
            this.form.ccListName = allNameS.join()
          } else {
            this.form.ccList = checkUser.map((item) => {
              return Number(item.id)
            })
            this.form.ccListName = perArray.join()
          }
          break
        default:
          perArray = checkUser.map((item) => {
            return item.name
          })
          const customAttribu = this.personnelType + 'customNameId'
          this.attribuForm[customAttribu] = Number(checkUser[0].id) // 存储人员id
          this.attribuForm[this.personnelType] = perArray.join()
          this.$forceUpdate()
          console.log(this.attribuFormChange())
          break
      }
    },

    // 动态属性添加
    addAttribute() {
      this.attributVisible = !this.attributVisible
    },
    handleAttribut(val) {
      this.initCheckAttr = val
      this.extendAttributeList = val.map((item) => JSON.parse(item))
      if (this.module == 2) this.handleConsequence(this.form.consequence)
    },

    // 试产模块 ，问题性质评分
    handleConsequence(value) {
      console.log(this.form.module)
      if (this.module !== 2) return
      switch (value) {
        case 0:
          this.attribuForm.ImpactScore = 10
          break
        case 1:
          this.attribuForm.ImpactScore = 3
          break
        case 2:
          this.attribuForm.ImpactScore = 1
          break
        case 3:
          this.attribuForm.ImpactScore = 0.3
          break
        default:
          this.attribuForm.ImpactScore = 0
          break
      }
    },

    // 动态属性日期类型
    attributDateType(fieldCode) {
      this.dateType = fieldCode
    },
    // 动态属性日期
    attributDate(dates, dateStrings) {
      if (this.dateType == 'forecastSolveTime') {
        this.form[this.dateType] = dateStrings
        console.log(this.form)
      } else {
        this.attribuForm[this.dateType] = dateStrings
        console.log(this.attribuForm)
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
          this.uploadInfo.file.status = 'done'
          const attachment = {
            attachmentId: res.data,
            fileName: file.name,
            attachmentType: 0,
          }
          this.attachment.push(attachment)
        } else {
          this.$message.error('上传失败：' + res.message)
          this.uploadInfo.file.status = 'error'
        }
      })
    },

    // 提交
    onSubmit() {
      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          this.form.extendAttribute = this.attribuFormChange()
          const { ccListName, currentAssignmentName, ...form } = this.form
          IssueDispatch(form)
            .then((res) => {
              if (res.success) {
                // 附件ID保存
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
                this.$message.success('问题分发成功')
                this.onback()
              } else {
                this.$message.warning(res.message)
              }
            })
            .catch(() => {
              this.$message.error('问题分发失败')
            })
        } else {
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
        const fieldName = item.fieldName
        if (fieldName === '样机说明') {
          item.value = this.attribuForm[fieldCode].join()
        } else {
          item.value = this.attribuForm[fieldCode]
        }
        item.issueId = item.issueId ?? 0
        return item
      })
      console.log('newEAL', newEAL)
      return JSON.stringify(newEAL)
    },

    // 返回
    onback() {
      if (sessionStorage.getItem('SET_CHECK_PATH')) {
        this.$router.push({ path: '/ssuissue' })
        sessionStorage.setItem('SET_CHECK_PATH', false) // 路径原路返回
      } else {
        this.$router.back()
      }
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