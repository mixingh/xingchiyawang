# 客户关系管理系统(CRM)
<p align="center">星驰雅望团队</p>
<p align="center">
  【代码仅供参考学习，禁止商用和非法用途，否则，一切后果请用户自负。】
</p>

<div align="center">

[![docker version][docker-version-image]][docker-version-url] [![docker pulls][docker-pulls-image]][docker-pulls-url] [![docker stars][docker-stars-image]][docker-stars-url] [![docker image size][docker-image-size-image]][docker-image-size-url] 

[docker-pulls-image]: https://img.shields.io/docker/pulls/136351/crm_api?style=flat
[docker-pulls-url]: https://hub.docker.com/r/136351/crm_api
[docker-version-image]: https://img.shields.io/docker/v/136351/crm_api?style=flat
[docker-version-url]: https://hub.docker.com/r/136351/crm_api/tags
[docker-stars-image]: https://img.shields.io/docker/stars/136351/crm_api?style=flat
[docker-stars-url]: https://hub.docker.com/r/136351/crm_api
[docker-image-size-image]: https://img.shields.io/docker/image-size/136351/crm_api?style=flat
[docker-image-size-url]: https://hub.docker.com/r/136351/crm_api
</div>


## 特别声明:
* 本仓库涉及的任何解锁和解密分析脚本或代码，仅用于测试和学习研究，禁止用于商业用途，不能保证其合法性，准确性，完整性和有效性，请根据情况自行判断.
* 请勿将本项目的任何内容用于商业或非法目的，否则后果自负.
* 如果任何单位或个人认为该项目的脚本可能涉嫌侵犯其权利，则应及时通知并提供身份证明，所有权证明，我们将在收到认证文件后删除相关代码.
* 任何以任何方式查看此项目的人或直接或间接使用本仓库项目的任何脚本的使用者都应仔细阅读此声明。保留随时更改或补充此免责声明的权利。一旦使用并复制了任何本仓库项目的规则，则视为您已接受此免责声明.
* 您必须在下载后的24小时内从计算机或手机中完全删除以上内容.
*  您使用或者复制了本仓库且本人制作的任何脚本，则视为已接受此声明，请仔细阅读
## 项目演示
项目演示地址： [http://crm.xingheng2026.top](http://crm.xingheng2026.top)  
![前端功能演示](https://m.360buyimg.com/babel/jfs/t1/97869/8/26240/565437/6264c54dE49afd62f/56accf8a8c5b5762.png)
![前端功能演示](https://m.360buyimg.com/babel/jfs/t1/217128/14/17977/169770/6264c54dE4afbc368/e699dd7fd4eb9663.png)
![前端功能演示](https://img30.360buyimg.com/pop/jfs/t1/113007/39/21701/93513/6236e08bE4664f874/2ed1cb139ca6e50b.png)
![前端功能演示](https://img30.360buyimg.com/pop/jfs/t1/197959/26/20042/69442/6236e224Eba4290b3/774d45b59edd38c0.png)
### 技术选型
#### 前端技术

| 技术       | 说明                  | 官网                                   |
| ---------- | --------------------- | -------------------------------------- |
| Vue        | 前端框架              | https://vuejs.org/                     |
| Vue-router | 路由框架              | https://router.vuejs.org/              |
| Vuex       | 全局状态管理框架      | https://vuex.vuejs.org/                |
| Element    | 前端UI框架            | https://element.eleme.io               |
| Axios      | 前端HTTP框架          | https://github.com/axios/axios         |
| v-charts   | 基于Echarts的图表框架 | https://v-charts.js.org/               |
| Js-cookie  | cookie管理工具        | https://github.com/js-cookie/js-cookie |
| nprogress  | 进度条控件            | https://github.com/rstacruz/nprogress  |
## 本地安装:
### 前端
#### windows平台
1.关于安装与使用Nginx请参考： [https://www.cnblogs.com/taiyonghai/p/9402734.html](https://www.cnblogs.com/taiyonghai/p/9402734.html)  
<br>
2.将crm目录下的dist下载至windows并修改nginx配置
> ```nginx.conf
> server {
>    # 声明监听的端口
>    listen 9669;
>    # 如有多域名映射到同一端口的，可以用server_name 区分，默认localhost
>    server_name localhost; 
>    # 匹配'/'开头的路径 注意设定 root路径是有dist的
>    location / {
>      # 指定文件的根目录，主要结尾不要带/
>      root /home/ubuntu/dist;
>      # 指定默认跳转页面尾 /index.html
>      index /index.html;
>    }
>    # 匹配'/adminApi'开头的路径进行跨域 ip和port自行替换
>    # adminApi 是vue.config.js中的proxy声明的
>    location /api {
>      proxy_pass http://81.71.19.235:9659;
>    }
>  }	
> ```
3.重启nginx，在浏览器地址栏输入：http://localhost:9669/ 即可运行！

#### linux平台
1.关于安装与使用Nginx请参考： [https://blog.csdn.net/cukw6666/article/details/107983709](https://blog.csdn.net/cukw6666/article/details/107983709)  
<br>
2.将crm目录下的dist下载并上传至linux
> ```git
> git@github.com:mixingh/xingchiyawang.git
> #或者手动下载上传
> ```

3.修改nginx配置（对应自己配置修改）
> ```nginx.conf
> vi /etc/nginx/nginx.conf
> #修改配置如下：
> server {
>    # 声明监听的端口
>    listen 9669;
>    # 如有多域名映射到同一端口的，可以用server_name 区分，默认localhost
>    server_name localhost; 
>    # 匹配'/'开头的路径 注意设定 root路径是有dist的
>    location / {
>      # 指定文件的根目录，主要结尾不要带/
>      root /home/ubuntu/dist;
>      # 指定默认跳转页面尾 /index.html
>      index /index.html;
>    }
>    # 匹配'/adminApi'开头的路径进行跨域 ip和port自行替换
>    # adminApi 是vue.config.js中的proxy声明的
>    location /api {
>      proxy_pass http://81.71.19.235:9659;
>    }
>  }	
> ```
4.检查并重启nginx：
> ```
> # 进入nginx 安装目录，yum安装的直接使用nginx代替sbin/nginx
> # 检查配置文件合法性 
>     sbin/nginx -t
> # nginx 热更新配置
>     sbin/nginx -s reload
> ```
5.在浏览器地址栏输入：http://localhost:9669/ 即可运行！



### 后端
#### windows平台
1.关于安装与使用Nginx请参考： [https://www.cnblogs.com/taiyonghai/p/9402734.html](https://www.cnblogs.com/taiyonghai/p/9402734.html)  

<br>

2.将crm_api目录下载至windows

<br>

3.在crm_api目录中打开cmd/powershell输入以下命令：
> ```cmd
> dotnet crm_api.dll
> ```

 如出现以下命令行:
> ```cmd-tip
> info: Microsoft.Hosting.Lifetime[0]
>       Now listening on: http://[::]:9660
> info: Microsoft.Hosting.Lifetime[0]
>       Application started. Press Ctrl+C to shut down.
> info: Microsoft.Hosting.Lifetime[0]
>       Hosting environment: Production
>info: Microsoft.Hosting.Lifetime[0]
> ```
4.在浏览器地址栏输入：http://localhost:9659/api/Salesmen 即可运行！


#### linux平台
以Ubuntu 20.4为例

<br>

1.关于安装与使用Nginx请参考： [https://blog.csdn.net/cukw6666/article/details/107983709](https://blog.csdn.net/cukw6666/article/details/107983709)

<br>

2.关于安装与使用Mysql请参考： [https://blog.csdn.net/YM_1111/article/details/107555383](https://blog.csdn.net/YM_1111/article/details/107555383)

<br>

3.安装.NET Core环境
> ```net
> #打开终端将 Microsoft 包签名密钥添加到受信任密钥列表，并添加包存储库。
> wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
> sudo dpkg -i packages-microsoft-prod.deb
> rm packages-microsoft-prod.deb
> #安装SDK（5.0版本）
> sudo apt-get update; \
>   sudo apt-get install -y apt-transport-https && \
>   sudo apt-get update && \
>   sudo apt-get install -y dotnet-sdk-5.0
> ```
4.将crm_api目录下载并上传至linux
> ```git
> git@github.com:mixingh/xingchiyawang.git
> #或者手动下载上传 
> ```
5.进入该目录后在root权限下执行
> ```
> #cd crm_api
> #运行api（后台运行）
> sudo nohup dotnet crm_api.dll &
> 运行api（无后台运行）
> sudo dotnet crm_api.dll
> ```
6.在浏览器地址栏输入：http://localhost:9659/api/Salesmen 即可运行！

## DOCKER:
* 仅以Linux平台演示
### 前端
### 安装说明

本项目已打包成`docker`镜像，拉取配置即可使用
> - docker安装方法不再赘述

#### 拉取并运行docker
> 
> 例如：
> ```dockerfile
> docker run -d \
>   -p 9669:9669 \
>   --name crm 136351/xingchiyawang:1.2
> ```
> 
注意：
 - 记得放行端口
#### 访问
这时候访问 `http://ip:port/` 就能访问API数据了


### 后端
### 安装说明

本项目已打包成`docker`镜像，拉取配置即可使用
> - docker安装方法不再赘述

#### 拉取并运行docker
> 
> 例如：
> ```dockerfile
> docker run -d \
>   -p 9660:9660 \
>   --name crm_api 136351/crm_api:1.1
> ```
> 
注意：
 - 记得放行端口
#### 访问
这时候访问 `http://ip:port/api/Salesmen` 就能访问API数据了

## DOCKER COMPOSE:
1.将crm_api dist下载并上传至Linux服务器中
<br>
2.进入crm_api目录并构建镜像：
> ```
> cd crm_api
> #进入crm_api目录
> docker-compose build
> #构建镜像
> Successfully built ff0721e80b7c
> Successfully tagged 3_crm:latest
> #出现这个就代表构建完成了。
> ```
3.启动运行
> ```
> docker-compose up -d#后台运行
> #启动容器
> 
> Creating 3_crm_api_1 ... done
> Creating 3_crm_1     ... done
> #出现这个就代表启动成功，可以访问了
> ```


# 更新说明
待更新..
