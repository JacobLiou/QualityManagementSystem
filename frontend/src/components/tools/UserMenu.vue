<template>
  <div class="user-wrapper">
    <div class="content-box">
      <!--<a href="" target="_blank">
        <span class="action">
          <a-icon type="question-circle-o"></a-icon>
        </span>
      </a>-->

      <span class="action" @click="toggleFullscreen">
        <a-icon type="fullscreen-exit" v-if="isFullscreen" />
        <a-icon type="fullscreen" v-else />
      </span>
      <notice-icon class="action" v-if="hasPerm('sysNotice:received')" />

      <a-dropdown>
        <span class="action ant-dropdown-link user-dropdown-menu">
          <a-avatar class="avatar" size="small" :src="avatar" />
          <span>{{ nickname }}</span>
        </span>
        <a-menu slot="overlay" class="user-dropdown-menu-wrapper">
          <a-menu-item key="4" v-if="mode === 'sidemenu'">
            <a @click="appToggled()">
              <a-icon type="swap" />
              <span>切换应用</span>
            </a>
          </a-menu-item>
          <!--<a-menu-item key="0">
            <router-link :to="{ name: 'center' }">
              <a-icon type="user"/>
              <span>个人中心</span>
            </router-link>
          </a-menu-item> -->
          <a-menu-item key="1">
            <router-link :to="{ name: 'settings' }">
              <a-icon type="setting" />
              <span>账户设置</span>
            </router-link>
          </a-menu-item>
          <a-menu-item key="2">
            <a @click="sendMessage()">
              <a-icon type="message" />
              <span>消息发送测试</span>
            </a>
          </a-menu-item>
          <a-menu-divider />
          <a-menu-item key="3">
            <a href="javascript:;" @click="handleLogout">
              <a-icon type="logout" />
              <span>退出登录</span>
            </a>
          </a-menu-item>
        </a-menu>
      </a-dropdown>
    </div>
    <a-modal title="切换应用" :visible="visible" :footer="null" :confirm-loading="confirmLoading" @cancel="handleCancel">
      <a-form :form="form1">
        <a-form-item :labelCol="labelCol" :wrapperCol="wrapperCol" label="选择应用">
          <a-menu mode="inline" :default-selected-keys="this.defApp" style="border-bottom:0px;lineHeight:55px;">
            <a-menu-item v-for="item in userInfo.apps" :key="item.code" style="top:0px;" @click="switchApp(item.code)">
              {{ item.name }}
            </a-menu-item>
          </a-menu>
        </a-form-item>
      </a-form>
    </a-modal>
  </div>
</template>

<script>
  import screenfull from 'screenfull'
  import NoticeIcon from '@/components/NoticeIcon'
  import {
    mapActions,
    mapGetters
  } from 'vuex'
  import {
    ALL_APPS_MENU
  } from '@/store/mutation-types'
  import Vue from 'vue'
  import {
    message
  } from 'ant-design-vue/es'
  import { messagesendtoAll,messagesendtosomeone } from '@/utils/messagesend'
  export default {
    name: 'UserMenu',
    components: {
      NoticeIcon,
      screenfull
    },
    props: {
      mode: {
        type: String,
        // sidemenu, topmenu
        default: 'sidemenu'
      }
    },
    data() {
      return {
        labelCol: {
          xs: {
            span: 24
          },
          sm: {
            span: 5
          }
        },
        wrapperCol: {
          xs: {
            span: 24
          },
          sm: {
            span: 16
          }
        },
        visible: false,
        confirmLoading: false,
        form1: this.$form.createForm(this),
        defApp: [],
        isFullscreen: false
      }
    },

    computed: {
      ...mapGetters(['token', 'nickname', 'avatar', 'userInfo'])
    },
    // 设置signalr令牌
    async mounted() {
      await this.$socket.authenticate(this.token)
    },
    methods: {
      ...mapActions(['Logout', 'MenuChange']),

      handleLogout() {
        this.$confirm({
          title: '提示',
          content: '真的要注销登录吗 ?',
          okText: '确定',
          cancelText: '取消',
          onOk: () => {
            return this.Logout({})
              .then(() => {
                setTimeout(() => {
                  window.location.reload()
                }, 16)
              })
              .catch(err => {
                this.$message.error({
                  title: '错误',
                  description: err.message
                })
              })
          },
          onCancel() {}
        })
      },

      /**
       * 打开切换应用框
       */
      appToggled() {
        this.visible = true
        this.defApp.push(Vue.ls.get(ALL_APPS_MENU)[0].code)
      },

      switchApp(appCode) {
        this.visible = false
        this.defApp = []
        const applicationData = this.userInfo.apps.filter(item => item.code === appCode)
        const hideMessage = message.loading('正在切换应用！', 0)
        this.MenuChange(applicationData[0])
          .then(res => {
            hideMessage()
            // eslint-disable-next-line handle-callback-err
          })
          .catch(err => {
            message.error('应用切换异常' + err)
          })
      },
      handleCancel() {
        this.form1.resetFields()
        this.visible = false
      },
      /* 全屏切换 */
      toggleFullscreen() {
        if (!screenfull.isEnabled) {
          message.error('您的浏览器不支持全屏模式')
          return
        }
        screenfull.toggle()
        if (screenfull.isFullscreen) {
          this.isFullscreen = false
        } else {
          this.isFullscreen = true
        }
      },
      // 发送消息测试
      sendMessage() {
        console.log(this.userInfo.id)
        //messagesendtoAll(Object.assign({ title: '测试标题', message: '这是消息内容', messagetype: 1 }))
        messagesendtosomeone(Object.assign({ userId: this.userInfo.id,title: '测试标题', message: '这是消息内容', messagetype: 1 }))
      },
    },
    // signalr接收的信息
    sockets: {
      ReceiveMessage(data) {
        switch (data.messagetype) {
          case 0:
            this.$notification.info({
              message: data.title,
              description: data.message,
              placement: 'bottomRight',
              duration: null
            })
          break
          case 1:
            this.$notification.success({
              message: data.title,
              description: data.message,
              placement: 'bottomRight',
              duration: null
            })
          break
          case 2:
            this.$notification.warning({
              message: data.title,
              description: data.message,
              placement: 'bottomRight',
              duration: null
            })
          break
          case 3:
            this.$notification.error({
              message: data.title,
              description: data.message,
              placement: 'bottomRight',
              duration: null
            })
          break
        }
      }
    }
  }
</script>

<style lang="less" scoped>
  .appRedio {
    border: 1px solid #91d5ff;
    padding: 10px 20px;
    background: #e6f7ff;
    border-radius: 2px;
    margin-bottom: 10px;
    color: #91d5ff;
    /*display: inline;
    margin-bottom:10px;*/
  }
</style>
