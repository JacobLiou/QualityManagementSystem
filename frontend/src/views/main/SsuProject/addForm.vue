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
        <a-form-item label="项目名称" :labelCol="labelCol" :wrapperCol="wrapperCol" has-feedback>
          <a-input
            placeholder="请输入项目名称"
            v-decorator="['projectName', { rules: [{ required: true, message: '请输入项目名称' }] }]"
          />
        </a-form-item>
        <a-form-item label="项目负责人" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <section class="from_chilen">
            <SelectUser
              v-decorator="['directorId', { rules: [{ required: true, message: '请输入并选择项目负责人' }] }]"
              title="请输入并选择项目负责人"
              @handlerSelectUser="handlerSelectUser"
              :userSelect="userSelect"
            ></SelectUser>
            <a-button @click="changePersonnel('directorId')"> 选择 </a-button>
          </section>
        </a-form-item>
        <a-form-item label="所属产品" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <SelectUser
            v-decorator="['productId', { rules: [{ required: true, message: '请输入并选择所属产品' }] }]"
            title="请输入并选择所属产品"
            @handlerSelectUser="handlerSelectProductId"
            :userSelect="productSelect"  
            queryType="SsuProductPage"          
          ></SelectUser>        
        </a-form-item>
        <a-form-item label="排序" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input-number
            placeholder="请输入排序"
            style="width: 100%"
            :min="0"
            v-decorator="['sort', { rules: [{ required: true, message: '请输入排序数值' }] }]"
          />
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
        <a-form-item v-if="optionType == 'edit'"><span v-decorator="['id']" v-show="false"></span></a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { SsuProjectAdd, SsuProjectEdit } from '@/api/modular/main/SsuProjectManage'
import { SsuProductList } from '@/api/modular/main/SsuProductManage'
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
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      directorName: '',
      mockData: [],
      targetKeys: [],
      productData:[],
      productName:'',
      titleName: '新增项目',
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
    productSelect(){
      return{
        id: this.form.getFieldsValue().productId,
        name: this.productName,
      }
    },
  },

  methods: {
    // 初始化方法,新增/编辑/复制
    AEC(record, type = 'add') {
      this.visible = true
      this.optionType = type      
      const userList = record?.userList
      let newUserIdList = this.editUserList(userList)
      switch (type) {
        case 'edit':
          setTimeout(() => {
            this.form.setFieldsValue({
              id: record.id,
              projectName: record.projectName,
              directorId: record.directorId,
              productId: record.productId,
              sort: record.sort,
              userIdList: newUserIdList.map((item) => item.id),
            })
            this.directorName = record.directorName // todo项目负责人name字段待定
            this.productName = record.productName
            console.log(this.form.getFieldsValue())
          }, 100)
          this.titleName = '编辑项目'
          break
        case 'copy':
          setTimeout(() => {
            this.form.setFieldsValue({
              projectName: record.projectName,
              directorId: record.directorId,
              productId: record.productId,
              sort: record.sort,
              userIdList: newUserIdList.map((item) => item.id),
            })
            this.directorName = record.directorName // todo项目负责人name字段待定
            this.productName = record.productName
          }, 100)
          this.titleName = '新增项目'
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

    // 模糊选中产品id
    handlerSelectProductId(valueObj) {
      this.form.setFieldsValue({
        productId: valueObj.value,
      })
      this.productName = valueObj.label
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
            this.EProject(values)
          } else {
            this.ACProject(values)
          }
        } else {
          this.confirmLoading = false
        }
      })
    },
    // 新增、复制项目
    ACProject(values) {
      SsuProjectAdd(values)
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
    // 编辑项目
    EProject(values) {
      SsuProjectEdit(values)
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
      this.productName = ''
      this.mockData = []
      this.targetKeys = []
      this.form.resetFields()
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
