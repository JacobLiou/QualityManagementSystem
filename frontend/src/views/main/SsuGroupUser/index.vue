<!--
 * @Author: 林伟群
 * @Date: 2022-05-16 16:28:46
 * @LastEditTime: 2022-05-16 18:55:13
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
        <a-tree :tree-data="treeData" default-expand-all @select="selectTree">
          <template #title="{ title }">
            <a-dropdown :trigger="['contextmenu']">
              <span>{{ title }}</span>
            </a-dropdown>
          </template>
        </a-tree>
      </a-card>
    </a-col>
    <!-- 列表 -->
    <a-col :md="16" :xs="24">
      <a-card class="user_list">
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
        <section class="list_buttom">
          <a-button type="primary" @click="userDefine">确定</a-button>
          <a-button @click="userCancel">返回</a-button>
        </section>
      </a-card>
    </a-col>
  </a-row>
  <!-- </section> -->
</template>

<script>
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
      treeData: [
        {
          title: '0-0',
          key: '0-0',
          children: [
            {
              title: '0-0-0',
              key: '0-0-0',
              children: [
                { title: '0-0-0-0', key: '0-0-0-0' },
                { title: '0-0-0-1', key: '0-0-0-1' },
                { title: '0-0-0-2', key: '0-0-0-2' },
              ],
            },
            {
              title: '0-0-1',
              key: '0-0-1',
              children: [
                { title: '0-0-1-0', key: '0-0-1-0' },
                { title: '0-0-1-1', key: '0-0-1-1' },
                { title: '0-0-1-2', key: '0-0-1-2' },
              ],
            },
          ],
        },
      ],
      columns: [
        {
          title: '序号',
          align: 'center',
          dataIndex: 'id',
        },
        {
          title: '姓名',
          align: 'center',
          dataIndex: 'userName',
        },
      ],
      userData: [
        {
          id: '1',
          userName: '理想',
        },
        {
          id: '1',
          userName: '理想',
        },
      ],
      checkRadio: false,
    }
  },
  computed: {
    rowSelection() {
      return {
        onChange: (selectedRowKeys, selectedRows) => {
          console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows)
        },
        type: this.checkRadio ? 'checkbox' : 'radio',
      }
    },
  },
  methods: {
    // 树节点选中
    selectTree(selectedKeys, e) {
      console.log(selectedKeys, e)
    },

    // 表格选择
    handleTableChange() {},

    // 确定
    userDefine() {},
    // 返回
    userCancel() {},
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
  .list_buttom {
    margin-top: 1.5em;
    text-align: right;
  }
}
</style>