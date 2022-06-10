<!--
 * @Author: 林伟群
 * @Date: 2022-05-17 14:31:45
 * @LastEditTime: 2022-05-31 19:02:58
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
        <div class="info_li" v-if="IssueDetailData.comment">
          <span class="li_title" v-if="IssueDetailData.comment">备注：</span>
          <div class="li_content">{{ IssueDetailData.comment }}</div>
        </div>
        <a-divider></a-divider>
        <section>
          <a-row :gutter="[6, 6]">
            <a-col :xl="3" :lg="6" :md="8" :xs="6" :key="item.currentKey" v-for="item in operationFilter">
              <a-button @click="operationType(item.operName)">{{ item.operName }}</a-button>
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
              <div class="li_content">{{ IssueDetailData.projectName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">产品名称：</span>
              <div class="li_content">{{ IssueDetailData.productName }}</div>
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
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">责任人：</span>
              <div class="li_content">{{ IssueDetailData.dispatcherName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">预计完成时间：</span>
              <div class="li_content">{{ IssueDetailData.forecastSolveTime }}</div>
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
              <div class="li_content">{{ IssueDetailData.discoverName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">发现日期：</span>
              <div class="li_content">{{ IssueDetailData.discoverTime }}</div>
            </div>
          </a-col>          
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">问题来源：</span>
              <div class="li_content">{{ sourceContent(IssueDetailData.source) }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">解决人：</span>
              <div class="li_content">{{ IssueDetailData.executorName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">解决日期：</span>
              <div class="li_content">{{ IssueDetailData.solveTime }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">实际完成时间：</span>
              <div class="li_content">{{ IssueDetailData.closeTime }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">解决版本：</span>
              <div class="li_content">{{ IssueDetailData.solveVersion }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">回归验证地点：</span>
              <div class="li_content">{{ IssueDetailData.verifierPlace }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">回归验证人：</span>
              <div class="li_content">{{ IssueDetailData.verifierName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">回归验证日期：</span>
              <div class="li_content">{{ IssueDetailData.validateTime }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">回归验证状态：</span>
              <div class="li_content">{{ IssueDetailData.validateTime }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">回归验证数量：</span>
              <div class="li_content">{{ IssueDetailData.count }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">回归验证批次：</span>
              <div class="li_content">{{ IssueDetailData.batch }}</div>
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
import { IssueDetail, IssueDelete,IssueReOpen } from '@/api/modular/main/SsuIssueManage'
import OperRecords from './componets/OperRecords.vue'
import ProblemSolve from './componets/ProblemSolve.vue'
import ProblemRecheck from './componets/ProblemRecheck.vue'
import ProblemValidate from './componets/ProblemValidate.vue'
import ProblemRedispatch from './componets/ProblemRedispatch.vue'
import ProblemHang from './componets/ProblemHang.vue'
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
  },
  data() {
    return {
      id: null,
      IssueDetailData: {},
      extendAttribute: [], // 自定义属性
      // 人员选择
      userVisible: false,
      personnelType: '',
    }
  },
  computed: {
    operationFilter() {
      if (this.IssueDetailData.status == undefined) return []
      let operationList = [
        {
          operName: '复制',
          operIcon: 'copy',
        },        
      ]
      const operationAdd = {
        0: [
          {
            operName: '分发',
            operIcon: 'select',
          },
          {
            operName: '删除',
            operIcon: 'delete',
          },
          {
            operName: '转交',
            operIcon: 'export',
          },
          {
            operName: '挂起',
            operIcon: 'minus-circle',
          },
          {
            operName: '编辑',
            operIcon: 'edit',
          },
        ],
        1: [
          {
            operName: '解决',
            operIcon: 'question-circle',
          },
        ],
        2: [          
          {
            operName: '复核',
            operIcon: 'reconciliation',
          },
        ],
        3: [
          {
            operName: '分发',
            operIcon: 'select',
          },
          {
            operName: '删除',
            operIcon: 'delete',
          },
          {
            operName: '转交',
            operIcon: 'export',
          },
          {
            operName: '挂起',
            operIcon: 'minus-circle',
          },
          {
            operName: '编辑',
            operIcon: 'edit',
          },
        ],
        4: [
          {
            operName: '重开启',
            operIcon: 'key',
          },
        ],
        5: [
          {
            operName: '重开启',
            operIcon: 'key',
          },
        ],
        6: [
          {
            operName: '重开启',
            operIcon: 'key',
          },
          {
            operName: '编辑',
            operIcon: 'edit',
          },
        ],
        7: [
          {
            operName: '验证',
            operIcon: 'safety-certificate',
          },         
        ],
      }
      const addList = operationAdd[Number(this.IssueDetailData.status)]
      const back = {
        operName: '返回',
        operIcon: 'rollback',
      }
      const newOperationList = [back, ...addList, ...operationList]
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
    },
    checkUserArray(checkUser) {
      console.log(checkUser)
      const perArray = checkUser.map((item) => {
        return item.name
      })
      console.log(checkUser)
      this.$refs.redispatchProblem.form.executor = Number(checkUser[0].id)
      this.$refs.redispatchProblem.form.executorName = perArray.join()
    },

    // 操作类型
    operationType(operName) {
      switch (operName) {
        case '返回':
          this.$router.back()
          break
        case '删除':
          this.problemDelect(this.IssueDetailData)
          break
        case '编辑':
          this.$router.push({ path: '/problemAdd', query: { editId: this.IssueDetailData.id } })
          break
        case '分发':
          // this.$store.commit('SET_CHECK_RECORD', record)
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
        default:
          break
      }
    },

    // 删除
    problemDelect(record) {
      const { id } = record
      const _this = this
      this.$confirm({
        content: '确定删除该问题',
        onOk() {
          IssueDelete({ id }).then((res) => {
            if (res.success) {
              _this.$message.success('删除成功')
              _this.$router.back()
            } else {
              _this.$message.error('删除失败')
            }
          })
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
          console.log('yuxinsong')
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