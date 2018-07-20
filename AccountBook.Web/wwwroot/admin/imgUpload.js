layui.use('upload', function () {
    var $ = layui.jquery
        upload1 = layui.upload,
        upload2 = layui.upload,
        upload3 = layui.upload;
    //普通图片上传
    var uploadInst = upload1.render({
        elem: '#btnUploadIcon',
        url: '/Admin/Upload',
        auto: true,
        multiple: false,
        size: 1024 * 1.05,  //kb
        accept: "images",
        exts: 'gif|jpg|jpeg|png|bmp|png',
        choose: function (obj) {  //上传前选择回调方法
            obj.preview(function (index, file, result) {
                        
            });
        },
        done: function (res) {      
            if (res.code == 200) {
                $('.layui-upload-listIcon').html(" <img src=" + res.msg + "  width='120' height='60' ><input type='hidden' id='LogoRectangle' name='LogoRectangle' value=" + res.msg +" />")              
            }
            else {
                return layer.msg(res.msg);
            }
        },      
    });
    //正放心图
    var uploadLogo = upload2.render({
        elem: '#btnUploadLogo',
        url: '/Admin/Upload',
        auto: true,
        multiple: false,
        size: 1024 * 1.05,  //kb
        accept: "images",
        exts: 'gif|jpg|jpeg|png|bmp|png',
        choose: function (obj) {  //上传前选择回调方法
            obj.preview(function (index, file, result) {            
            });
        },
        done: function (res) {
            if (res.code == 200) {
                $('.layui-upload-listLogo').html("<img src=" + res.msg + "  width='60' height='60' ><input type='hidden' id='LogoSquare' name='LogoSquare' value=" + res.msg +" />");                
            }
            else {
                return layer.msg(res.msg);
            }
        }
    });
    var uploadShowImg = upload3.render({       
            elem: "#btnUploadShowImg"
            , url: '/Admin/Upload'
            , multiple: true,
            accept: "images",
            exts: 'gif|jpg|jpeg|png|bmp|png',
            size: 1024 * 1.05 //限制文件大小，单位 KB
            , before: function (obj) {
                obj.preview(function (index, file, result) {
                });
            },
            done: function (res) {
                if (res.code == 200) {
                    document.getElementById('demo2').innerHTML += "<div class='div_img' ><a href=" + res.msg + " target='_blank'><img src=" + res.msg + " class='layui-upload-img' /></a><span  class='delete-img'>删除</span><input type='hidden'  value=" + res.msg + "  />                                                  </div>";
                    //document.getElementById('realyImg').innerHTML += "<input type='hidden'  value=" + res.msg + "  />";
                    $('.delete-img').each(function (index) {
                        var _this = this;
                        $(this).click(function () {
                            $(_this).parent('div').remove();
                           // $('#realyImg input').eq(index).remove();
                        })
                    })
                    return;
                }
                if (res.code == 500) {
                    layer.msg(res.msg);
                    return;
                }
            }
        });
});