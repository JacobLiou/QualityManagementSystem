<!--
 * @Author: 林伟群
 * @Date: 2022-05-16 16:28:46
 * @LastEditTime: 2022-05-20 16:07:38
 * @LastEditors: 林伟群
 * @Description: 人员组成员管理
 * @FilePath: \frontend\src\views\main\SsuGroupUser\index.vue
-->
<template>
  <a-row :gutter="[12, 6]">
    <!-- 列表 -->
    <a-col :md="3" :xs="24">
      <a-card class="user1">
        <a-menu v-model="current" mode="inline">
          <a-menu-item :key="item.currentKey" v-for="item in currentList"> {{ item.currentName }}</a-menu-item>
        </a-menu>
      </a-card>
    </a-col>
    <!-- 树 -->
    <a-col :md="5" :xs="24">
      <a-card class="user1">
        <a-spin :spinning="treeLoading">
          <a-tree :tree-data="treeData" default-expand-all @select="selectTree" :replaceFields="replaceFields">
          </a-tree>
        </a-spin>
      </a-card>
    </a-col>
    <!-- 列表 -->
    <a-col :md="16" :xs="24">
      <a-card class="user_list">
        <a-spin :spinning="tabLoading">
          <a-table
            :columns="columns"
            :row-key="
              (record, index) => {
                return index
              }
            "
            :data-source="userData"
            :pagination="false"
            @change="handleTableChange"
            :scroll="{ y: 'calc(100vh - 120px)' }"
            :row-selection="rowSelection"
          >
          </a-table>
          <section class="list_button" v-if="backPath !== ''">
            <a-button type="primary" @click="userDefine" class="button1">确定</a-button>
            <a-button @click="userCancel">返回</a-button>
          </section>
        </a-spin>
      </a-card>
    </a-col>
  </a-row>
  <!-- </section> -->
</template>

<script>
import { SsuProductList, SsuProductusers } from '@/api/modular/main/SsuProductManage'
import { SsuProjectList, SsuProjectusers } from '@/api/modular/main/SsuProjectManage'
import { getOrgTree, getOrgUserList } from '@/api/modular/system/orgManage'
import { SsuGroupList, SsuGroupusers } from '@/api/modular/main/SsuGroupManage'
import qs from 'qs'

export default {
  data() {
    return {
      currentList: [
        {
          currentKey: 'productId',
          currentName: '产品',
        },
        {
          currentKey: 'projectId',
          currentName: '项目',
        },
        {
          currentKey: 'departmentId',
          currentName: '部门',
        },
        {
          currentKey: 'personnelID',
          currentName: '人员',
        },
      ],
      current: ['productId'],
      treeData: [],
      columns: [
        {
          title: '序号',
          align: 'center',
          dataIndex: 'index',
        },
        {
          title: '姓名',
          align: 'center',
          dataIndex: 'name',
        },
      ],
      userData: [],
      checkRadio: false,
      replaceFields: {
        key: 'id',
      },
      treeLoading: false,
      tabLoading: false,
      checkUser: [],
      backPath: '',
    }
  },
  created() {
    const { personnelType, backPath } = this.$store.state.record
    this.backPath = backPath
    this.checkRadio = personnelType == 'ccList' ? true : false
  },
  computed: {
    rowSelection() {
      return {
        onChange: (selectedRowKeys, selectedRows) => {
          this.checkUser = selectedRows
        },
        type: this.checkRadio ? 'checkbox' : 'radio',
      }
    },
  },
  watch: {
    current: {
      handler(val) {
        this.userData = []
        this.treeLoading = true
        switch (val[0]) {
          case 'productId':
            this.getSsuProductList()
            break
          case 'projectId':
            this.getProjectList()
            break
          case 'departmentId':
            this.getDepartmentList()
            break
          case 'personnelID':
            this.getPersonnelList()
            break
          default:
            break
        }
      },
      immediate: true,
    },
  },
  methods: {
    // 树节点选中
    selectTree(key) {
      this.tabLoading = true
      switch (this.current[0]) {
        case 'productId':
          this.getproductuserList(key)
          break
        case 'projectId':
          this.getProjectUserList(key)
          break
        case 'departmentId':
          this.getDepartmentUserList(key)
          break
        case 'personnelID':
          this.getPersonnelUserList(key)
          break
        default:
          break
      }
    },

    // 表格选择
    handleTableChange(value) {
      console.log(value)
    },

    // 获取产品列表
    getSsuProductList() {
      SsuProductList()
        .then((res) => {
          if (res.success) {
            const resultData = res.data
            this.treeData = resultData.map((item) => {
              item.title = item.productName
              return item
            })
          } else {
            this.treeData = []
            this.$message.error('产品列表获取失败')
          }
        })
        .catch(() => {
          this.treeData = []
          this.$message.error('产品列表获取失败')
        })
        .finally(() => {
          this.cancelLoading()
        })
    },

    // 获取项目列表
    getProjectList() {
      SsuProjectList()
        .then((res) => {
          if (res.success) {
            const resultData = res.data
            this.treeData = resultData.map((item) => {
              item.title = item.projectName
              return item
            })
          } else {
            this.treeData = []
            this.$message.error('项目列表获取失败')
          }
        })
        .catch(() => {
          this.treeData = []
          this.$message.error('项目列表获取失败')
        })
        .finally(() => {
          this.cancelLoading()
        })
    },

    // 获取部门列表
    getDepartmentList() {
      getOrgTree()
        .then((res) => {
          if (res.success) {
            this.treeData = res.data
          } else {
            this.treeData = []
            this.$message.error('部门列表获取失败')
          }
        })
        .catch(() => {
          this.treeData = []
          this.$message.error('部门列表获取失败')
        })
        .finally(() => {
          this.cancelLoading()
        })
    },

    // 获取人员列表
    getPersonnelList() {
      SsuGroupList()
        .then((res) => {
          if (res.success) {
            const resultData = res.data
            this.treeData = resultData.map((item) => {
              item.title = item.groupName
              return item
            })
          } else {
            this.treeData = []
            this.$message.error('人员列表获取失败')
          }
        })
        .catch(() => {
          this.treeData = []
          this.$message.error('人员列表获取失败')
        })
        .finally(() => {
          this.cancelLoading()
        })
    },

    // 根据部门id获取人员列表
    getDepartmentUserList(key) {
      const id = key[0]
      getOrgUserList({orgId:id})
        .then((res) => {
          console.log(res)
          if (res.success) {
            this.userData = res.data
            this.userData.forEach((item, index) => (item.index = index + 1))
          }
        })
        .catch(() => {
          this.$message.error('用户列表获取失败')
        })
        .finally(() => {
          this.cancelLoading()
        })
    },
    // 根据项目ID获取成员列表
    getProjectUserList(key) {
      const id = key[0]
      SsuProjectusers({projectId:id})
        .then((res) => {
          if (res.success) {
            this.userData = res.data
            this.userData.forEach((item, index) => (item.index = index + 1))
          }
        })
        .catch(() => {
          this.$message.error('用户列表获取失败')
        })
        .finally(() => {
          this.cancelLoading()
        })
    },
    // 根据产品id获取人员列表
    getproductuserList(key) {
      const id = key[0]
      SsuProductusers({productId:id})
        .then((res) => {
          if (res.success) {
            this.userData = res.data
            this.userData.forEach((item, index) => (item.index = index + 1))
          }
        })
        .catch(() => {
          this.$message.error('用户列表获取失败')
        })
        .finally(() => {
          this.cancelLoading()
        })
    },
    // 根据人员ID获取人员列表
    getPersonnelUserList(key) {
      const id = key[0]
      SsuGroupusers({groupId:id})
        .then((res) => {
          if (res.success) {
            this.userData = res.data
            this.userData.forEach((item, index) => (item.index = index + 1))
          }
        })
        .catch(() => {
          this.$message.error('用户列表获取失败')
        })
        .finally(() => {
          this.cancelLoading()
        })
    },

    // 消除加载
    cancelLoading() {
      this.tabLoading = false
      this.treeLoading = false
    },

    // 确定
    userDefine() {
      this.$store.commit('SET_CHECK_USER', this.checkUser)
      this.$store.commit('SET_CHECK_TRUE', true)
      this.$router.replace({ path: this.backPath })
    },
    // 返回
    userCancel() {
      this.$store.commit('SET_CHECK_USER', [])
      this.$store.commit('SET_CHECK_TRUE', false)
      this.$router.replace({ path: this.backPath })
    },
  },
}
</script>

<style lang="less" scoped>
.user1 {
  width: 100%;
}
.user_list {
  width: 100%;
  height: calc(100vh - 120px);
  .list_button {
    margin-top: 1.5em;
    text-align: right;
    .button1 {
      margin-right: 2em;
    }
  }
}
</style>