<!--
 * @Author: 林伟群
 * @Date: 2022-05-19 10:30:06
 * @LastEditTime: 2022-06-23 09:58:50
 * @LastEditors: 林伟群
 * @Description: 问题增加页面
 * @FilePath: \frontend\src\views\main\SsuIssue\problemAdd.vue
-->
<template>
  <a-card class="add">
    <div class="add_title">{{ isStorage ? '问题新增' : '问题编辑' }}</div>
    <section class="form_1">
      <a-form-model ref="ruleForm" :labelCol="labelCol" :wrapperCol="wrapperCol" :model="form" :rules="rules">
        <a-form-model-item label="产品线">
          <a-select v-model="form.productId" placeholder="请选择产品">
            <a-select-option v-for="(item, index) in productData" :key="index" :value="item.id">{{
              item.productName
            }}</a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="产品项目" prop="projectId">
          <a-select v-model="form.projectId" placeholder="请选择所属产品项目" @change="projectChange">
            <a-select-option v-for="(item, index) in projectData" :key="index" :value="item.id">{{
              item.projectName
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
        <a-form-model-item label="当前指派" prop="currentAssignment">
          <section class="from_chilen">
            <a-select v-model="form.currentAssignment" placeholder="请选择指派人">
              <a-select-option v-for="item in currentAssignmentData" :key="item.id" :value="Number(item.id)">{{
                item.name
              }}</a-select-option>
            </a-select>
          </section>
        </a-form-model-item>
        <a-form-model-item label="性质" prop="consequence">
          <a-select v-model="form.consequence" placeholder="请选择问题性质" @change="handleConsequence">
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
            <SelectUser
              title="请输入发现人"
              @handlerSelectUser="handlerSelectUser"
              selectType="discoverName"
              :userSelect="userSelectDis"
            ></SelectUser>
            <a-button @click="changePersonnel('discover')"> 选择 </a-button>
          </section>
        </a-form-model-item>
        <a-form-model-item label="发现日期" prop="discoverTime">
          <a-date-picker
            style="width: 100%"
            placeholder="请选择发现日期"
            v-model="form.discoverTime"
            @change="attributDate"
            @focus="attributDateType('discoverTime', form.discoverTime)"
            :disabled-date="disabledDate"
          />
        </a-form-model-item>
        <a-form-model-item label="抄送" prop="ccListName">
          <section class="from_chilen" :title="form.ccListName">
            <!-- <a-input v-model="form.ccListName" placeholder="请选择抄送人" disabled /> -->
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
                @focus="attributDateType(attItem.fieldCode, attribuForm[attItem.fieldCode])"
                :disabled-date="disabledDate"
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
                <a-row style="width: 100%" :gutter="[2, 6]">
                  <a-col :span="10" v-for="(item, index) in checkAttArray(attItem.fieldCode, true)" :key="index">
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
            <a-upload
              :customRequest="customRequest"
              :multiple="true"
              :showUploadList="true"
              name="file"
              @change="handleChange"
            >
              <a-button icon="upload">附件上传</a-button>
            </a-upload>
          </a-form-item>
        </section>
        <section class="add_once">
          <a-form-model-item :wrapper-col="wrapperCol3">
            <a-button type="primary" @click="onSubmit"> {{ addName }} </a-button>
            <a-button style="margin-left: 10px" @click="resetForm"> 重置 </a-button>
            <a-button style="margin-left: 10px" type="primary" @click="onStorage" v-if="isStorage"> 暂存 </a-button>
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
      :initCheckAttr="initCheckAttr"
    ></AttributCheck>
  </a-card>
</template>

<script>
import { SsuProjectList } from '@/api/modular/main/SsuProjectManage'
import { SsuProductList } from '@/api/modular/main/SsuProductManage'
import { IssueAdd, IssueAttachmentSaveId, IssueDetail, IssueEdit } from '@/api/modular/main/SsuIssueManage'
import { getresponsibilityuser } from '@/api/modular/system/orgManage'
import VueQuillEditor from './componets/VueQuillEditor.vue'
import CheckUserList from './componets/CheckUserList.vue'
import AttributCheck from './componets/AttributCheck.vue'
import { sysFileInfoUpload } from '@/api/modular/system/fileManage'
import SelectUser from './componets/SelectUser.vue'
import SelectUserMore from './componets/SelectUserMore.vue'
import moment from 'moment'

export default {
  components: { CheckUserList, VueQuillEditor, AttributCheck, SelectUser, SelectUserMore },
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
        currentAssignment: undefined, // 指派人ID
        currentAssignmentName: '', // 指派人名字
        consequence: undefined, // 性质
        // 下边不是必传字段
        source: null, // 问题来源
        discover: '', // 发现人
        discoverName: '', // 发现人名字
        discoverTime: null, // 发现日期
        ccList: [], // 抄送人
        ccListName: '', // 抄送人名字
        description: '', // 详情
        extendAttribute: '', // 新增字段
      },
      rules: {
        title: [{ required: true, message: '请输入问题简述', trigger: 'blur' }],
        currentAssignment: [{ required: true, message: '请选择指派人', trigger: 'change' }],
        projectId: [{ required: true, message: '请选择所属项目', trigger: 'change' }],
        module: [{ required: true, message: '请选择模块', trigger: 'change' }],
        issueClassification: [{ required: true, message: '请选择问题分类', trigger: 'change' }],
        consequence: [{ required: true, message: '请选择性质', trigger: 'change' }],
      },
      projectData: [], // 项目list
      productData: [], // 产品list
      moduleData: [], // 模块list
      issueClassificationData: [], // 问题分类list
      currentAssignmentData: [], // 指派人list
      consequenceData: [], // 问题性质
      sourceData: [], // 问题来源
      ccListData: [], // 抄送人数组
      discoverData: [], // 发现人数组
      dispatcherData: [], // 分发人数组
      userVisible: false, // 抽屉显示
      personnelType: '', // 选择的人
      attributVisible: false, // 选择属性
      extendAttributeList: [], // 新增的属性
      initCheckAttr: [],
      attribuForm: {}, // 新增属性表单
      moduleType: '', // 新增类型属性
      dateType: '', // 时间类型
      editDate: '',
      attachment: [], // 附件参数
      isEdit: false, // 问题编辑
      status: -1, // 状态
      // oldDescription: '', // 编辑时旧的描述
      copyAdd: 0, // 是否为复制
      uploadInfo: {}, // 文件上传详情
    }
  },
  created() {
    const id = this.$route.query.editId
    this.copyAdd = this.$route.query.copyAdd
    if (id) {
      this.isEdit = true
      // 编辑
      this.getIssueDetail(id)
    }
    if (!this.isEdit) {
      this.form.discoverTime = moment().format('YYYY-MM-DD')
    }
    this.initList()
    this.getFromData()
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
    isStorage() {
      if (this.copyAdd == 1) return true
      return this.status == -1 ? true : false
    },
    addName() {
      if (this.copyAdd == 1) return '添加'
      return this.status == -1 ? '添加' : '确定'
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
    userSelectDis() {
      return {
        id: this.form.discover,
        name: this.form.discoverName,
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
  },
  methods: {
    // 获取详情
    getIssueDetail(id) {
      IssueDetail({ id })
        .then((res) => {
          if (res.success) {
            this.form.description = res.data.description
            this.initEditData(res.data)
            if (!res.data.extendAttribute) return
            const extendAttributeS = JSON.parse(res.data.extendAttribute)
            if (extendAttributeS.length === 0) return
            this.extendAttributeList = extendAttributeS.filter((item) => {
              return Boolean(item.value)
            })
            this.$forceUpdate()
            this.extendAttributeList.forEach((item) => {
              if (item.fieldName == '样机说明') {
                const arrTrue = item.value.indexOf(',')
                this.attribuForm[item.fieldCode] = arrTrue == -1 ? [] : item.value.split(',')
              } else {
                this.attribuForm[item.fieldCode] = item.value
              }
            })
            this.initCheckAttr = this.extendAttributeList.map((item) => {
              const { value, issueId, ...newItem } = item
              return JSON.stringify(newItem)
            })
          }
        })
        .catch(() => {
          this.$message.error('描述获取失败')
        })
    },
    // 动态属性渲染
    attribuCheck() {
      this.$forceUpdate()
    },
    // 编辑数据初始化
    initEditData(checkRecord) {
      this.form.id = checkRecord.id
      this.form.title = checkRecord.title
      this.form.projectId = checkRecord.projectId // 项目编号
      this.form.productId = checkRecord.productId // 产品编号
      this.form.module = checkRecord.module // 模块
      this.form.issueClassification = checkRecord.issueClassification // 问题分类
      this.form.dispatcher = checkRecord.dispatcherId // 分发人ID（指派人）
      this.form.dispatcherName = checkRecord.dispatcherName // 分发人名字
      this.form.consequence = checkRecord.consequence // 性质
      this.form.source = checkRecord.source // 问题来源
      this.form.discover = checkRecord.discoverId // 发现人
      this.form.discoverName = checkRecord.discoverName // 发现人名字
      this.form.discoverTime = checkRecord.discoverTime // 发现日期
      this.form.ccList = JSON.parse(checkRecord.ccList) // 抄送人
      this.form.ccListName = JSON.parse(checkRecord.ccListName)?.join() // 抄送人名字
      this.status = checkRecord.status
      this.getresponsibility()
      this.form.currentAssignment = checkRecord.currentAssignment
      this.form.currentAssignmentName = checkRecord.currentAssignmentName
    },
    // 列表初始化
    initList() {
      this.moduleData = this.$options.filters['dictData']('issue_module')
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
      this.getresponsibility()
      // 根据模块id获取人员列表
    },
    // 根据项目id获取人员列表
    projectChange() {
      this.getresponsibility()
    },

    // 根据项目ID和模块id获取人员列表
    getresponsibility() {
      const projectId = this.form.projectId
      const { module } = this.form
      const moduleObj = this.moduleData.filter((item) => item.code == module)
      if (projectId != undefined && moduleObj[0]?.id != undefined) {
        this.form.currentAssignment = undefined
        this.form.currentAssignmentName = ''
        getresponsibilityuser({ projectId, modularId: moduleObj[0]?.id })
          .then((res) => {
            if (res.success) {
              this.currentAssignmentData = res.data.map((item) => {
                return {
                  id: item.id,
                  name: item.name,
                }
              })
              console.log(this.currentAssignmentData)
            } else {
              this.currentAssignmentData = []
              this.$message.warning('当前指派人列表获取失败')
            }
          })
          .catch(() => {
            this.currentAssignmentData = []
            this.$message.warning('当前指派人列表获取失败')
          })
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
    attributDateType(fieldCode, date) {
      if (this.isEdit && this.dateType !== fieldCode) {
        this.editDate = date
      }
      this.dateType = fieldCode
    },
    // 动态属性日期
    attributDate(dates, dateStrings) {
      if (this.dateType == 'discoverTime') {
        this.form[this.dateType] = dateStrings
      } else {
        this.attribuForm[this.dateType] = dateStrings
      }
    },
    // 禁止部分时间
    disabledDate(current) {
      // 编辑和增加
      if (this.isEdit && this.editDate && this.copyAdd == 0) {
        return current.valueOf() < moment(this.editDate).valueOf()
      } else {
        return current && current < moment().subtract(1, 'days')
      }
    },

    // 模糊搜索选中人员
    handlerSelectUser(valueObj) {
      const { selectType } = valueObj
      let perArray = []
      switch (selectType) {
        case 'currentAssignment': // 分发指派人
          this.form.currentAssignment = valueObj.value
          this.form.currentAssignmentName = valueObj.label
          break
        case 'discover': // 发现
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
        case 'dispatcher': // 分发
          this.dispatcherData = checkUser
          perArray = checkUser.map((item) => {
            return item.name
          })
          this.form.dispatcherName = perArray.join()
          this.form.dispatcher = Number(checkUser[0].id)
          break
        case 'discover': // 发现
          this.discoverData = checkUser
          perArray = checkUser.map((item) => {
            return item.name
          })
          this.form.discoverName = perArray.join()
          this.form.discover = Number(checkUser[0].id)
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
            this.form.ccListName = allNameS.join()
          } else {
            this.form.ccList = checkUser.map((item) => {
              return Number(item.id)
            })
            this.form.ccListName = perArray.join()
          }
          break
        case 'currentAssignment': // 指派
          this.ccListData = checkUser
          perArray = checkUser.map((item) => {
            return item.name
          })
          this.form.currentAssignmentName = perArray.join()
          this.form.currentAssignment = Number(checkUser[0].id)
          this.$forceUpdate()
          break
        default:
          perArray = checkUser.map((item) => {
            return item.name
          })
          const customAttribu = this.personnelType + 'customNameId'
          this.attribuForm[customAttribu] = Number(checkUser[0].id)
          this.attribuForm[this.personnelType] = perArray.join()
          this.$forceUpdate()
          break
      }
      this.$forceUpdate()
    },
    handleChange(info) {
      if (info.file.status === 'removed') {
        const fileIndex = this.uploadInfo.fileList.findIndex((item) => {
          if (item.uid === info.file.uid) {
            return item
          }
        })
        this.attachment.splice(fileIndex, 1)
      }
      this.uploadInfo = info
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
          this.uploadInfo.file.status = 'error'
        }
      })
    },

    // 提交
    onSubmit() {
      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          this.form.extendAttribute = this.attribuFormChange()
          if (this.isStorage) {
            this.addIssue(this.form)
          } else {
            this.editIssue(this.form)
          }
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
      return JSON.stringify(newEAL)
    },

    // 新增/缓存
    addIssue(form) {
      IssueAdd(form)
        .then((res) => {
          if (res.success) {
            const issueId = res.data.id
            // 附件ID保存
            if (this.attachment.length !== 0) {
              const parameter = {
                attachments: this.attachment,
                issueId: issueId,
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
            this.$message.success(this.form.isTemporary ? '问题暂存成功' : '问题增加成功')
            this.onback()
          } else {
            this.$message.warning(res.message)
          }
        })
        .catch(() => {
          this.$message.error(this.form.isTemporary ? '问题暂存失败' : '问题增加失败')
        })
    },

    // 编辑
    editIssue(form) {
      IssueEdit(form)
        .then((res) => {
          if (res.success) {
            // 附件ID保存
            if (this.attachment.length !== 0) {
              const parameter = {
                attachments: this.attachment,
                issueId: form.id,
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
            this.$message.success('问题编辑成功')
            this.onback()
          } else {
            this.$message.warning(res.message)
          }
        })
        .catch(() => {
          this.$message.error('问题编辑失败')
        })
    },

    // 重置
    resetForm() {
      this.$refs.ruleForm.resetFields()
      // this.form.description = this.oldDescription
      this.extendAttributeList = [] // 新增的属性
      this.initCheckAttr = []
      this.attribuForm = {} // 新增属性表单
    },
    // 暂存
    onStorage() {
      this.form.isTemporary = true
      this.onSubmit()
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
      this.initCheckAttr = val
      this.extendAttributeList = val.map((item) => JSON.parse(item))
      this.extendAttributeList.forEach((item) => {
        if (item.fieldDataType == 'DateTime' && this.attribuForm[item.fieldCode] == undefined) {
          this.attribuForm[item.fieldCode] = moment().format('YYYY-MM-DD')
        }
      })
      if (this.form.module == 2) this.handleConsequence(this.form.consequence)
    },
    // 试产模块 ，问题性质评分
    handleConsequence(value) {
      console.log(this.form.module)
      if (this.form.module !== 2) return
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