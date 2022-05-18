<!--
 * @Author: 林伟群
 * @Date: 2022-05-17 14:31:45
 * @LastEditTime: 2022-05-18 19:55:46
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
          <div class="li_content">{{ record.title }}</div>
        </div>
        <div class="info_li">
          <span class="li_title">问题详情：</span>
          <div class="li_content">{{ IssueDetailData.description }}</div>
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
            <a-col
              :xl="3"
              :lg="6"
              :md="8"
              :xs="6"
              :key="item.currentKey"
              v-for="item in operationFilter(record.status)"
            >
              <a-button @click="operationType(item.operName)">{{ item.operName }}</a-button>
            </a-col>
          </a-row>
        </section>
      </a-card>
      <!-- 历史记录 -->
      <a-card class="info">
        <OperRecords></OperRecords>
      </a-card>
    </a-col>
    <!-- 基本信息 -->
    <a-col :md="10" :xs="24">
      <a-card class="list">
        <a-row :gutter="[6, 6]" align="middle" type="flex">
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">项目名称：</span>
              <div class="li_content">{{ record.projectName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">产品名称：</span>
              <div class="li_content">{{ record.productName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">模块：</span>
              <div class="li_content">{{ moduleContent(record.module) }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">问题分类：</span>
              <div class="li_content">{{ ICFContent(record.issueClassification) }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">状态：</span>
              <div class="li_content">{{ statusContent(record.status) }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">责任人：</span>
              <div class="li_content">{{ record.dispatcherName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">预计完成时间：</span>
              <div class="li_content">{{ record.forecastSolveTime }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">性质：</span>
              <div class="li_content">{{ consequenceContent(record.consequence) }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">发现人：</span>
              <div class="li_content">{{ record.discoverName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">发现日期：</span>
              <div class="li_content">{{ record.discoverTime }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">发现日期：</span>
              <div class="li_content">{{ record.discoverTime }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">问题来源：</span>
              <div class="li_content">{{ sourceContent(record.source) }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">解决人：</span>
              <div class="li_content">{{ record.executorName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">解决日期：</span>
              <div class="li_content">{{ record.solveTime }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">实际完成时间：</span>
              <div class="li_content">{{ record.solveTime }}</div>
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
              <div class="li_content">{{ record.verifierPlace }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">回归验证人：</span>
              <div class="li_content">{{ record.verifierName }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">回归验证日期：</span>
              <div class="li_content">{{ record.validateTime }}</div>
            </div>
          </a-col>
          <a-col :xl="12" :xs="24"
            ><div class="info-li">
              <span class="li_title">回归验证状态：</span>
              <div class="li_content">{{ record.validateTime }}</div>
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
        <a-row :gutter="[24, 12]" align="middle" type="flex" v-if="extendAttribute.length !== 0">
          <a-col :xl="12" :xs="24" v-for="(item, index) in extendAttribute" :key="index"
            ><div class="info-li">
              <span class="li_title">{{ item.fieldName }}</span>
              <div class="li_content">{{ item.value }}</div>
            </div>
          </a-col>
        </a-row>
      </a-card>
    </a-col>
  </a-row>
</template>

<script>
import { IssueDetail, IssueDelete } from '@/api/modular/main/SsuIssueManage'
import OperRecords from './componets/OperRecords.vue'
export default {
  components: {
    OperRecords,
  },
  data() {
    return {
      IssueDetailData: {},
      record: {},
      extendAttribute: [], // 自定义属性
    }
  },
  created() {
    this.record = this.$store.state.record.checkRecord
    const id = this.$route.query.id
    if (id !== undefined) {
      this.getIssueDetail(id)
    }
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

    // 操作权限
    operationFilter(state) {
      let operationList = [
        {
          operName: '编辑',
          operIcon: 'edit',
        },
        {
          operName: '删除',
          operIcon: 'delete',
        },
      ]
      const operationAdd = {
        0: [
          {
            operName: '分发',
            operIcon: 'select',
          },
        ],
        1: [
          {
            operName: '解决',
            operIcon: 'question-circle',
          },

          {
            operName: '转交',
            operIcon: 'export',
          },
          {
            operName: '挂起',
            operIcon: 'minus-circle',
          },
        ],
        2: [
          {
            operName: '验证',
            operIcon: 'safety-certificate',
          },
          {
            operName: '转交',
            operIcon: 'export',
          },
        ],
        3: [
          {
            operName: '转交',
            operIcon: 'export',
          },
          {
            operName: '挂起',
            operIcon: 'minus-circle',
          },
        ],
        4: [],
        5: [
          {
            operName: '分发',
            operIcon: 'select',
          },
        ],
      }
      const addList = operationAdd[String(state)]
      const newOperationList = [
        {
          operName: '返回',
          operIcon: 'rollback',
        },
        ...addList,
        ...operationList,
      ]
      return newOperationList
    },

    // 操作类型
    operationType(operName) {
      switch (operName) {
        case '返回':
          this.$router.back()
          break
        case '删除':
          this.problemDelect(this.record)
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

    // 获取详情
    getIssueDetail(id) {
      IssueDetail({ id })
        .then((res) => {
          if (res.success) {
            this.IssueDetailData = res.data
            // this.extendAttribute = this.changeExtendAttribute(res.data.extendAttribute)
          } else {
            this.$message.error('问题详情查看失败')
          }
        })
        .catch((err) => {
          this.$message.error('问题详情查看失败')
        })
    },

    // 自定义数据数据改造
    changeExtendAttribute(extendAttribute) {
      if (extendAttribute == null) return []
      const newExtendAttribute = extendAttribute.map((item) => {
        if (item.filedDataType == 'enum') {
          const contentArray = this.$options.filters['dictData'](item.fieldCode)
          const data = contentArray.find((it) => it.code == item.value)
          item.value = data
        }
        return item
      })
      return newExtendAttribute
    },
  },
}
</script>

<style lang="less" scoped>
.info {
  width: 100%;
  margin-bottom: 1.5em;
  .info_li {
    display: flex;
    justify-self: flex-start;
    align-self: flex-start;
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
      font-size: 1.1em;
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