<!--
 * @Author: 林伟群
 * @Date: 2022-05-17 14:31:45
 * @LastEditTime: 2022-06-22 15:27:51
 * @LastEditors: 林伟群
 * @Description: 问题详情
 * @FilePath: \frontend\src\views\main\SsuIssue\problemInfo.vue
-->

<template>
  <a-row :gutter="[12, 6]">
    <!-- 详情页面 -->
    <a-col :md="14" :xs="24">
      <!-- 问题详情 -->
      <a-card class="info">
        <div class="info_li">
          <span class="li_title">问题简述：</span>
          <div class="li_content">{{ IssueDetailData.title }}</div>
        </div>
        <div class="info_li">
          <span class="li_title">问题详情：</span>
          <div class="li_content" v-html="IssueDetailData.description"></div>
        </div>
        <div class="info_li">
          <span class="li_title">原因分析：</span>
          <div class="li_content">{{ IssueDetailData.reason }}</div>
        </div>
        <div class="info_li" v-if="IssueDetailData.measures">
          <span class="li_title">解决措施：</span>
          <div class="li_content">{{ IssueDetailData.measures }}</div>
        </div>
        <div class="info_li" v-if="IssueDetailData.result">
          <span class="li_title">验证情况：</span>
          <div class="li_content">{{ IssueDetailData.result }}</div>
        </div>
        <div class="info_li" v-if="IssueDetailData.hangupReason">
          <span class="li_title">挂起原因：</span>
          <div class="li_content">{{ IssueDetailData.hangupReason }}</div>
        </div>
        <div class="info_li" v-if="IssueDetailData.attachmentList !== null">
          <span class="li_title">附件： <a-spin size="small" v-if="cardLoading" /></span>
          <div
            class="li_content li_file"
            v-for="(iFile, index) in IssueDetailData.attachmentList"
            :key="iFile.attachmentId"
            @click="downFile(iFile.attachmentId, index)"
            :title="'附件下载:' + iFile.fileName"
          >
            {{ iFile.fileName }}
          </div>
        </div>
        <div class="info_li" v-if="IssueDetailData.comment">
          <span class="li_title" v-if="IssueDetailData.comment">备注：</span>
          <div class="li_content">{{ IssueDetailData.comment }}</div>
        </div>
        <a-divider></a-divider>
        <section>
          <a-row :gutter="[6, 6]">
            <a-col :xl="3" :lg="6" :md="8" :xs="6" :key="item.currentKey" v-for="item in operationFilter">
              <a-button @click="operationType(item.operName)" :type="item.operType">{{ item.operName }}</a-button>
            </a-col>
          </a-row>
        </section>
      </a-card>
      <!-- 历史记录 -->
      <a-card class="info">
        <OperRecords :id="id"></OperRecords>
      </a-card>
    </a-col>
    <!-- 基本信息 -->
    <a-col :md="10" :xs="24">
      <a-card class="list">
        <a-row :gutter="[6, 6]" align="middle" type="flex">
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">项目名称：</span>
              <div class="li_content">{{ IssueDetailData.projectName || '--' }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">产品名称：</span>
              <div class="li_content">{{ IssueDetailData.productName || '--' }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">模块：</span>
              <div class="li_content">{{ moduleContent(IssueDetailData.module) }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">问题分类：</span>
              <div class="li_content">{{ ICFContent(IssueDetailData.issueClassification) }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">状态：</span>
              <div class="li_content">{{ statusContent(IssueDetailData.status) }}</div>
            </div>
          </a-col> <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">待办人</span>
              <div class="li_content">{{ IssueDetailData.currentAssignmentName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">责任人：</span>
              <div class="li_content">{{ IssueDetailData.dispatcherName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">预计解决日期：</span>
              <div class="li_content">{{ IssueDetailData.forecastSolveTime || '--' }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">性质：</span>
              <div class="li_content">{{ consequenceContent(IssueDetailData.consequence) }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">发现人：</span>
              <div class="li_content">{{ IssueDetailData.discoverName || '--' }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">发现日期：</span>
              <div class="li_content">{{ IssueDetailData.discoverTime || '--' }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">问题来源：</span>
              <div class="li_content">{{ sourceContent(IssueDetailData.source) || '--' }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">解决人：</span>
              <div class="li_content">{{ IssueDetailData.executorName || '--' }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">解决日期：</span>
              <div class="li_content">{{ IssueDetailData.solveTime || '--' }}</div>
            </div>
          </a-col>         
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">解决版本：</span>
              <div class="li_content">{{ IssueDetailData.solveVersion || '--' }}</div>
            </div>
          </a-col>
         
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">回归验证人：</span>
              <div class="li_content">{{ IssueDetailData.verifierName || '--' }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">回归验证日期：</span>
              <div class="li_content">{{ IssueDetailData.validateTime || '--' }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">回归验证状态：</span>
              <div class="li_content">{{ validationStatusContent(IssueDetailData.validationStatus) || '--' }}</div>
            </div>
          </a-col>
         
            <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">问题关闭时间：</span>
              <div class="li_content">{{ IssueDetailData.closeTime || '--' }}</div>
            </div>
          </a-col>
        </a-row>
        <a-row :gutter="[24, 12]" type="flex" v-if="extendAttribute.length !== 0">
          <a-col :xl="12" :xs="24" v-for="(item, index) in extendAttribute" :key="index">
            <!-- 单选 -->
            <div class="info-li" v-if="item.fieldDataType == 'bool'">
              <span class="li_title">{{ item.fieldName }}:</span>
              <div class="li_content">
                <a-radio-group
                  v-model="item.value"
                  v-for="item in checkAttArray(item.fieldCode, item.value, true)"
                  :key="item.label"
                >
                  <a-radio :value="item.value"> {{ item.label }} </a-radio>
                </a-radio-group>
              </div>
            </div>
            <!-- 多选 -->
            <div class="info-li" v-else-if="item.fieldDataType == 'enum' && item.fieldName === '样机说明'">
              <span class="li_title">{{ item.fieldName }}:</span>
              <div class="li_content">
                <a-checkbox-group :value="item.value == '' ? [] : item.value.split(',')">
                  <a-row style="width: 100%" :gutter="[2, 2]">
                    <a-col
                      :span="24"
                      v-for="(item, index) in checkAttArray(item.fieldCode, item.value, true)"
                      :key="index"
                    >
                      <a-checkbox :value="item.value">
                        {{ item.label }}
                      </a-checkbox>
                    </a-col>
                  </a-row>
                </a-checkbox-group>
              </div>
            </div>
            <!-- 下拉 -->
            <div class="info-li" v-else-if="item.fieldDataType == 'enum' && item.fieldName !== '样机说明'">
              <span class="li_title">{{ item.fieldName }}:</span>
              <div class="li_content">{{ checkAttArray(item.fieldCode, item.value) }}</div>
            </div>
            <div class="info-li" v-else>
              <span class="li_title">{{ item.fieldName }}:</span>
              <div class="li_content">{{ item.value }}</div>
            </div>
          </a-col>
        </a-row>
      </a-card>
    </a-col>
    <!-- 问题解决弹窗 -->
    <ProblemSolve ref="problemSolve"></ProblemSolve>
    <!-- 问题复核 -->
    <ProblemRecheck ref="recheckProblem"></ProblemRecheck>
    <!-- 问题验证 -->
    <ProblemValidate ref="validateProblem"></ProblemValidate>
    <!-- 问题转交 -->
    <ProblemRedispatch ref="redispatchProblem" @changePersonnel="changePersonnel"></ProblemRedispatch>
    <!-- 问题挂起 -->
    <ProblemHang ref="hangProblem"></ProblemHang>
    <!-- 问题关闭 -->
    <ProblemClose ref="closeProblem"></ProblemClose>
    <!-- 选择人员 -->
    <CheckUserList
      class="checkUser"
      :userVisible="userVisible"
      :personnelType="personnelType"
      @checkUserArray="checkUserArray"
    ></CheckUserList>
  </a-row>
</template>

<script>
import { IssueDetail, IssueDelete, IssueReOpen, IssueSendur } from '@/api/modular/main/SsuIssueManage'
import { sysFileInfoDownload } from '@/api/modular/system/fileManage'
import OperRecords from './componets/OperRecords.vue'
import ProblemSolve from './componets/ProblemSolve.vue'
import ProblemRecheck from './componets/ProblemRecheck.vue'
import ProblemValidate from './componets/ProblemValidate.vue'
import ProblemRedispatch from './componets/ProblemRedispatch.vue'
import ProblemHang from './componets/ProblemHang.vue'
import ProblemClose from './componets/ProblemClose.vue'
import CheckUserList from './componets/CheckUserList.vue'
export default {
  components: {
    OperRecords,
    ProblemSolve,
    ProblemRecheck,
    ProblemValidate,
    ProblemRedispatch,
    ProblemHang,
    CheckUserList,
    ProblemClose,
  },
  data() {
    return {
      id: null,
      IssueDetailData: {},
      extendAttribute: [], // 自定义属性
      // 人员选择
      userVisible: false,
      personnelType: '',
      cardLoading: false,
    }
  },
  computed: {
    operationFilter() {
      if (this.IssueDetailData.status == undefined)
        return [
          {
            operName: '返回',
            operIcon: 'rollback',
             operType: 'default'
          },
        ]
      // const { admintype } = this.$store.state.user // 获取用户类型 1是超级用户
      const operationArray = [
       
        
        {
          operName: '复制',
          operIcon: 'copy',
          operType: 'default'
        },
        {
          operName: '编辑',
          operIcon: 'edit',
          operType: 'default'
        },
        {
          operName: '详情',
          operIcon: 'file-done',
          operType: 'default'
        },
         {
          operName: '分发',
          operIcon: 'select',
          operType: 'primary'
        },
        {
          operName: '转交',
          operIcon: 'export',
          operType: 'default'
        },
        {
          operName: '解决',
          operIcon: 'question-circle',
          operType: 'primary'
        },
        {
          operName: '复核',
          operIcon: 'reconciliation',
          operType: 'primary'
        },
        {
          operName: '验证',
          operIcon: 'safety-certificate',
          operType: 'primary'
        },
        {
          operName: '关闭',
          operIcon: 'close-circle',
          operType: 'default'
        },
        {
          operName: '挂起',
          operIcon: 'minus-circle',
           operType: 'default'
        },
        {
          operName: '重开启',
          operIcon: 'key',
           operType: 'default'
        },
        {
          operName: '催办',
          operIcon: 'bell',
           operType: 'default'
        },
        {
          operName: '删除',
          operIcon: 'delete',
           operType: 'default'
        },
      ]
      const newOperationList = [
        {
          operName: '返回',
          operIcon: 'rollback',
           operType: 'default'
        },
      ]
      this.IssueDetailData.btnList?.forEach((item) => {
        if (item != 2) {
          newOperationList.push(operationArray[item])
        }
      })
      return newOperationList
    },
  },
  created() {
    this.id = this.$route.query.id
    if (this.id) {
      this.getIssueDetail()
    }
  },
  provide() {
    return { getProblemList: this.getIssueDetail }
  },
  methods: {
    moduleContent(text) {
      if (text == undefined) return
      const contentArray = this.$options.filters['dictData']('issue_module')
      const data = contentArray.find((item) => item.code == text)
      return data.name
    },
    ICFContent(text) {
      if (text == undefined) return
      const contentArray = this.$options.filters['dictData']('issue_classification')
      const data = contentArray.find((item) => item.code == text)
      return data.name
    },
    statusContent(text) {
      if (text == undefined) return
      const contentArray = this.$options.filters['dictData']('issue_status')
      const data = contentArray.find((item) => item.code == text)
      return data.name
    },
    validationStatusContent(text) {
      if (text == undefined) return
      const contentArray = this.$options.filters['dictData']('issue_validationstatus')
      const data = contentArray.find((item) => item.code == text)
      return data.name
    },
    sourceContent(text) {
      if (text == undefined) return
      const contentArray = this.$options.filters['dictData']('issue_source')
      const data = contentArray.find((item) => item.code == text)
      return data.name
    },
    consequenceContent(text) {
      if (text == undefined) return
      const contentArray = this.$options.filters['dictData']('issue_consequence')
      const data = contentArray.find((item) => item.code == text)
      return data.name
    },

    // 人员选择
    changePersonnel(value) {
      this.personnelType = value
      this.userVisible = !this.userVisible
    },
    checkUserArray(checkUser) {
      // console.log(checkUser)
      const perArray = checkUser.map((item) => {
        return item.name
      })
      // console.log(checkUser)
      this.$refs.redispatchProblem.form.executor = Number(checkUser[0].id)
      this.$refs.redispatchProblem.form.executorName = perArray.join()
    },

    // 操作类型
    operationType(operName) {
      switch (operName) {
        case '返回':
          if (sessionStorage.getItem('SET_CHECK_PATH')) {
            this.$router.push({ path: '/ssuissue' })
            sessionStorage.setItem('SET_CHECK_PATH', false) // 路径原路返回
          } else {
            this.$router.back()
          }
          break
        case '删除':
          this.problemDelectSend(this.IssueDetailData, '删除')
          break
        case '催办':
          this.problemDelectSend(this.IssueDetailData, '催办')
          break
        case '编辑':
          this.$router.push({ path: '/problemAdd', query: { editId: this.IssueDetailData.id } })
          break
        case '分发':
          this.$router.push({ path: '/problemDistribure', query: { distributeId: this.IssueDetailData.id } })
          break
        case '解决':
          this.$refs.problemSolve.initSolv(this.IssueDetailData, false)
          break
        case '复核':
          this.$refs.recheckProblem.recheckForm(this.IssueDetailData, false)
          break
        case '验证':
          this.$refs.validateProblem.initValidate(this.IssueDetailData, false)
          break
        case '转交':
          this.$refs.redispatchProblem.initRedispatch(this.IssueDetailData, false)
          break
        case '挂起':
          this.$refs.hangProblem.initHang(this.IssueDetailData, false)
          break
        case '重开启':
          this.problemOpen(this.IssueDetailData)
          break
        case '复制':
          this.$router.push({ path: '/problemAdd', query: { editId: this.IssueDetailData.id, copyAdd: 1 } })
          break
        case '关闭':
          this.$refs.closeProblem.initClose(this.IssueDetailData, false)
          break
        default:
          break
      }
    },

    // 删除\催办
    problemDelectSend(record, text) {
      const { id } = record
      const _this = this
      this.$confirm({
        content: '确定' + text + '该问题',
        onOk() {
          if (text == '删除') {
            IssueDelete({ id })
              .then((res) => {
                if (res.success) {
                  _this.$message.success(text + '成功')
                  _this.$router.back()
                } else {
                  _this.$message.warning(res.message)
                }
              })
              .catch(() => {
                _this.$message.error('删除失败')
              })
          } else {
            IssueSendur({ id })
              .then((res) => {
                if (res.success) {
                  _this.$message.success(text + '成功')
                  _this.$router.back()
                } else {
                  _this.$message.warning(res.message)
                }
              })
              .catch(() => {
                _this.$message.error('催办失败')
              })
          }
        },
        onCancel() {},
      })
    },

    //  重新开启
    problemOpen(record) {
      const { id } = record
      const _this = this
      this.$confirm({
        content: '确定重新开启',
        onOk() {
          IssueReOpen({ id })
            .then((res) => {
              if (res.success) {
                _this.$message.success('重开启成功')
                _this.getIssueDetail()
              } else {
                _this.$message.warning(res.message)
              }
            })
            .catch(() => {
              _this.$message.error('重开启失败')
            })
        },
        onCancel() {},
      })
    },

    // 获取详情
    getIssueDetail() {
      IssueDetail({ id: this.id })
        .then((res) => {
          if (res.success) {
            this.IssueDetailData = res.data
            if (!res.data.extendAttribute) return
            this.extendAttribute = this.changeExtendAttribute(res.data.extendAttribute)
          }
        })
        .catch(() => {
          this.$message.error('问题详情查看失败')
        })
    },

    // 自定义数据数据改造
    changeExtendAttribute(extendAttribute) {
      if (!extendAttribute) return []
      const extendAttributeArray = JSON.parse(extendAttribute)
      const newExtendAttribute = extendAttributeArray.filter((item) => {
        if (item.value !== '') {
          if (item.filedDataType == 'enum') {
            const contentArray = this.$options.filters['dictData'](item.fieldCode)
            const data = contentArray.find((it) => it.code == item.value)
            item.value = data
          }
          return item
        }
      })
      return newExtendAttribute
    },
    // 自定义数据渲染
    checkAttArray(fieldCode, value, check = false) {
      const attArray = this.$options.filters['dictData'](fieldCode)
      const ValueObj = attArray.find((item) => item.code == value)
      if (!check) return ValueObj?.name ?? ''
      const newAttArray = attArray.map((item) => {
        return { label: item.name, value: item.code }
      })
      return newAttArray
    },

    // 附件下载
    downFile(id, index) {
      this.cardLoading = true
      sysFileInfoDownload({ id })
        .then((res) => {
          this.cardLoading = false
          this.downloadfile(res)
          // eslint-disable-next-line handle-callback-err
        })
        .catch((err) => {
          this.cardLoading = false
          this.$message.error('下载错误：获取文件流错误' + err)
        })
    },
    downloadfile(res) {
      var blob = new Blob([res.data], { type: 'application/octet-stream;charset=UTF-8' })
      var contentDisposition = res.headers['content-disposition']
      var patt = new RegExp('filename=([^;]+\\.[^\\.;]+);*')
      var result = patt.exec(contentDisposition)
      var filename = result[1]
      var downloadElement = document.createElement('a')
      var href = window.URL.createObjectURL(blob) // 创建下载的链接
      var reg = /^["](.*)["]$/g
      downloadElement.style.display = 'none'
      downloadElement.href = href
      downloadElement.download = decodeURI(filename.replace(reg, '$1')) // 下载后文件名
      document.body.appendChild(downloadElement)
      downloadElement.click() // 点击下载
      document.body.removeChild(downloadElement) // 下载完成移除元素
      window.URL.revokeObjectURL(href)
    },
  },
}
</script>

<style lang="less" scoped>
.info {
  width: 100%;
  margin-bottom: 1.5em;
  .info_li {
    margin-bottom: 1.5em;
    margin-right: 0.5em;
    .li_title {
      width: 80px;
      flex-shrink: 0;
      font-size: 1.1em;
      font-weight: 700;
      text-align: right;
    }
    .li_content {
      text-indent: 2em;
      /deep/ img {
        width: 100%;
      }
    }
    .li_file {
      cursor: pointer;
      color: #2096db;
    }
  }
  .title {
    width: 80px;
    flex-shrink: 0;
    font-size: 1.1em;
    font-weight: 700;
  }
  ul {
    margin-bottom: 2em;
    li {
      margin: 2em 0;
      margin-left: -1em;
      list-style: none;
    }
  }
  .step {
    margin-left: 16px;
  }
}
.list {
  .info-li {
    display: flex;
    justify-self: flex-start;
    align-self: flex-start;
    margin-bottom: 1.5em;
    margin-right: 0.5em;
    .li_title {
      width: 100px;
      flex-shrink: 0;
      font-size: 1em;
      font-weight: 700;
    }
    .li_content {
      font-size: 1em;
    }
  }
}
</style>