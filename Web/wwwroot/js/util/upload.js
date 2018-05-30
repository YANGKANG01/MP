define(['jquery', 'util/common','plupload', 'layer'], function ($, common) {
    var token,
        domain,
        container,
        uploader,
        _ = {
            init: function (settings, callback) {
                _.render(settings, callback);
            },
            /**
             * Request token from server for uploading.
             * @param {Function} [callback]
             */
            reqToken: function (callback) {
                common.get(API.qiniuToken, {}, function (data) {
                    token = data['uptoken'];
                    domain = data['domain'];

                    $.isFunction(callback) && callback(token, domain);
                });
            },
            render: function (settings, callback) {
                var target = settings['browse_button'];
                var uploader = new plupload.Uploader({ //实例化一个plupload上传对象
                    browse_button: target,
                    url: 'upload.html',
                    flash_swf_url: '/plugin/plupload/js/Moxie.swf',
                    silverlight_xap_url: '/plugin/plupload/js/Moxie.xap',
                    filters: {
                        mime_types: [ //只允许上传图片文件
                            { title: "图片文件", extensions: "jpg,gif,png" }
                        ]
                    }
                });
                uploader.init(); //初始化

                //绑定文件添加进队列事件
                uploader.bind('FilesAdded', function (uploader, files) {
                    for (var i = 0, len = files.length; i < len; i++) {
                        var file_name = files[i].name; //文件名
                        //构造html来更新UI<p class="progress"></p><p class="file-name">' + file_name + '</p>
                        var html = '<li id="file-' + files[i].id + '"></li>';
                        $(html).appendTo('#upload-img-items');
                        !function (i) {
                           _.previewImage(files[i], function (imgsrc) {
                               $('#file-' + files[i].id).append('<img style="width:50px;height:50px;float:left" src="' + imgsrc + '" />');
                            })
                        }(i);
                    }
                    _.renderImage(files, target, files)
                });


            },
            //plupload中为我们提供了mOxie对象
            //有关mOxie的介绍和说明请看：https://github.com/moxiecode/moxie/wiki/API
            //如果你不想了解那么多的话，那就照抄本示例的代码来得到预览的图片吧
            previewImage: function (file, callback) {//file为plupload事件监听函数参数中的file对象,callback为预览图片准备完成的回调函数
                if (!file || !/image\//.test(file.type)) return; //确保文件是图片
                if (file.type == 'image/gif') {//gif使用FileReader进行预览,因为mOxie.Image只支持jpg和png
                    var fr = new mOxie.FileReader();
                    fr.onload = function () {
                        callback(fr.result);
                        fr.destroy();
                        fr = null;
                    }
                    fr.readAsDataURL(file.getSource());
                } else {
                    var preloader = new mOxie.Image();
                    preloader.onload = function () {
                        preloader.downsize(300, 300);//先压缩一下要预览的图片,宽300，高300
                        var imgsrc = preloader.type == 'image/jpeg' ? preloader.getAsDataURL('image/jpeg', 80) : preloader.getAsDataURL(); //得到图片src,实质为一个base64编码的数据
                        callback && callback(imgsrc); //callback传入的参数为预览图片的url
                        preloader.destroy();
                        preloader = null;
                    };
                    preloader.load(file.getSource());
                }
            },
        };
    return {
        init: _.init
    };
});