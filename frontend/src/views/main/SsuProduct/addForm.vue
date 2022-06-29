<template>
  <a-modal
    :title="titleName"
    :width="900"
    :visible="visible"
    :confirmLoading="confirmLoading"
    @ok="handleSubmit"
    @cancel="handleCancel"
  >
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item label="产品名称" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input
            placeholder="请输入产品名称"
            v-decorator="['productName', { rules: [{ required: true, message: '请输入产品名称' }] }]"
          />
        </a-form-item>
        <a-form-item label="产品负责人" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <section class="from_chilen">
            <SelectUser
              v-decorator="['directorId']"
              title="请输入并选择产品负责人"
              @handlerSelectUser="handlerSelectUser"
              :userSelect="userSelect"
            ></SelectUser>
            <a-button @click="changePersonnel('directorId')"> 选择 </a-button>
          </section>
        </a-form-item>
        <a-form-item label="状态" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select
            style="width: 100%"
            placeholder="请选择状态"
            v-decorator="['status', { rules: [{ required: true, message: '请选择状态！' }] }]"
          >
            <a-select-option v-for="(item, index) in statusData" :key="index" :value="Number(item.code)">{{
              item.name
            }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="产品分类" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select
            style="width: 100%"
            placeholder="请选择产品分类"
            allowClear="true"
            v-decorator="['classificationId']"
          >
            <a-select-option v-for="(item, index) in classificationIdData" :key="index" :value="Number(item.code)">{{
              item.name
            }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="人员关联">
          <a-transfer
            v-decorator="['userIdList']"
            :data-source="mockData"
            show-search
            :filter-option="filterOption"
            :target-keys="targetKeys"
            :render="(item) => item.title"
            @change="handleChange"
            @search="handleSearch"
            :list-style="{
              width: '40%',
              height: '300px',
            }"
          />
        </a-form-item>
        <a-form-item v-show="false"><a-input v-decorator="['id']" /></a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { SsuProductAdd, SsuProductEdit } from '@/api/modular/main/SsuProductManage'
import { getfuzzyusers } from '@/api/modular/main/SsuGroupManage'
import SelectUser from '@/components/SelectUser/SelectUser'

export default {
  components: { SelectUser },
  data() {
    return {
      labelCol: {
        xs: { span: 24 },
        sm: { span: 5 },
      },
      wrapperCol: {
        xs: { span: 24 },
        sm: { span: 15 },
      },
      statusData: [],
      projectData: [],
      classificationIdData: [],
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      directorName: '',
      projectName: '',
      mockData: [],
      targetKeys: [],
      titleName: '新增产品线',
      optionType: 'add', // 操作类型
    }
  },
  computed: {
    userSelect() {
      return {
        id: this.form.getFieldsValue().directorId,
        name: this.directorName,
      }
    },
  },
  methods: {
    // 初始化方法
    AEC(record, type = 'add') {
      this.visible = true      
      const statusOption = this.$options
      this.statusData = statusOption.filters['dictData']('product_status')
      const classificationIdOption = this.$options
      this.classificationIdData = classificationIdOption.filters['dictData']('product_classification')
      this.optionType = type
      const userList = record?.userList
      let newUserIdList = this.editUserList(userList)
      switch (type) {
        case 'edit':
          setTimeout(() => {
            this.form.setFieldsValue({
              id: record.id,
              productName: record.productName,              
              status: record.status,
              classificationId: record.classificationId,
              directorId: record.directorId,
              userIdList: newUserIdList.map((item) => item.id),
            })
            this.projectName = record.projectName
            this.directorName = record.directorName
          }, 100)
          this.titleName = '编辑产品线'
          break
        case 'copy':
          setTimeout(() => {
            this.form.setFieldsValue({
              productName: record.productName,              
              status: record.status,
              classificationId: record.classificationId,
              directorId: record.directorId,
              userIdList: newUserIdList.map((item) => item.id),
            })
            this.projectName = record.projectName
            this.directorName = record.directorName
          }, 100)
          this.titleName = '新增产品线'
          break
        default:
          setTimeout(() => {
            this.form.setFieldsValue({
              userIdList: [],
            })
          }, 100)
          break
      }
    },
    // 编辑关联人员列表处理
    editUserList(userList) {
      if (!userList?.length) return []
      this.mockData = userList.map((item) => {
        const data = {
          key: item.id,
          title: item.name,
          description: item.name,
          chosen: true,
        }
        return data
      })
      this.targetKeys = this.mockData.filter((item) => item.chosen == true).map((item) => item.key)
      const newUserIdList = userList.map((item) => {
        return {
          id: item.id,
          name: item.name,
        }
      })
      return newUserIdList
    },
    // 模糊搜索选中人员
    handlerSelectUser(valueObj) {
      this.form.setFieldsValue({
        directorId: valueObj.value,
      })
      this.directorName = valueObj.label
    },
    changePersonnel(value) {
      this.$emit('changePersonnel', value)
    },  
    filterOption(inputValue, option) {
      return option.description.indexOf(inputValue) > -1
    },
    handleChange(targetKeys, direction, moveKeys) {
      this.targetKeys = targetKeys
    },
    // 搜索框变化的数据
    handleSearch(dir, value) {
      if (dir == 'left') {
        // 远程搜索获取数据
        getfuzzyusers({ name: value }).then((res) => {
          if (res.success) {
            const data = res.data.map((item, i) => {
              const data = {
                key: item.id,
                title: item.name,
                description: item.name,
                chosen: false,
              }
              return data
            })
            const newData = data.map((item) => JSON.stringify(item))
            const mockData = this.mockData.map((item) => JSON.stringify(item))
            const newMockData = [...new Set([...mockData, ...newData])]
            this.mockData = newMockData.map((item) => JSON.parse(item))
          }
        })
        this.targetKeys = this.targetKeys
      } else if (dir == 'right') {
        this.targetKeys = this.targetKeys
      }
    },

    /**
     * 提交表单
     */
    handleSubmit() {
      const {
        form: { validateFields },
      } = this
      this.confirmLoading = true
      validateFields((errors, values) => {        
        if (!errors) {
          if (this.optionType == 'edit') {            
            this.EProduct(values)
          } else {
            this.ACProduct(values)
          }
        } else {
          this.confirmLoading = false
        }
      })
    },
    // 新增、复制产品
    ACProduct(values) {
      SsuProductAdd(values)
        .then((res) => {
          if (res.success) {
            this.$message.success('新增成功')
            this.confirmLoading = false
            this.$emit('ok', values)
            this.handleCancel()
          } else {
            this.$message.error('新增失败：' + JSON.stringify(res.message))
          }
        })
        .finally((res) => {
          this.confirmLoading = false
        })
    },
    // 编辑产品
    EProduct(values) {      
      SsuProductEdit(values)
        .then((res) => {
          if (res.success) {
            this.$message.success('编辑成功')
            this.confirmLoading = false
            this.$emit('ok', values)
            this.handleCancel()
          } else {
            this.$message.error('编辑失败：' + JSON.stringify(res.message))
          }
        })
        .finally((res) => {
          this.confirmLoading = false
        })
    },
    handleCancel() {
      this.directorName = ''
      this.projectName = ''
      this.form.resetFields()
      this.mockData = []
      this.targetKeys = []
      this.visible = false
    },
  },
}
</script>
<style lang="less" scoped>
.from_chilen {
  display: flex;
}
</style>
