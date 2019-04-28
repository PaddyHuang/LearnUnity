
    layui.config({
        base: "/Content/admin/js/"
    }).use(['form', 'vue', 'ztree', 'layer', 'utils', 'jquery', 'table', 'droptree', 'openauth', 'element', 'upload','common'], function () {

        var $ = layui.jquery
            , common = layui.common
            , element = layui.element
            , form = layui.form
            , upload = layui.upload;
        layui.droptree("/UserSession/QueryNavList", "#ParentName", "#BelongToID", false);

        $(document).on("click","li.layui-timeline-item", function () {
            var el = $(this).find('i.layui-timeline-axis');
            if (el) {
                $('i.selected').removeClass('selected');
                el.addClass('selected');
            }
            $(this).siblings("li.layui-timeline-item").each(function () {

                var el1 = $(this).find('i.layui-timeline-axis');

                el1.removeClass('selected');
            })
        })


        $('.layui-tab-item > .del').click(function () {

            var selected = $('i.selected').parent();

            if (selected && selected.length > 0)
            {
                //询问框

                var inel = selected.find('input.index');

                if (inel && inel.length > 0) {


                    layer.confirm('确定删除 内容项：' + inel.val(), {
                        btn: ['确定', '取消'] //按钮
                    }, function () {

                        var parentel = selected.parent();

                        selected.remove();

                        var elems = parentel.find('li.layui-timeline-item');

                        if (elems.length > 0)
                        {

                            var ccnum = 1;
                            elems.each(function () {


                                

                                var spanel = $(this).find('span.tindex');

                                var inputel = $(this).find('input.index');
                                if (spanel && spanel.length > 0)
                                {
                                    spanel.text(ccnum);
                                }

                                if (inputel && inputel.length > 0)
                                {
                                    inputel.val(ccnum);
                                }

                                ccnum++
                            })
                        }



                        layer.msg("移除成功")
                    }, function () {
                        return;
                    });
                }
                else
                {
                    layer.msg("请选中要移除的节点");
                }


      
            }


        })


    

        //普通图片上传
        var uploadInst = upload.render({
            elem: '#test1'
            , url: common.renderUrl('/QiniuClient/CreateDir', {
                modelID: '@ViewBag.ModleID'
            }) 
            , field: 'modelthumb'
            ,accept:'file'
            , exts:''
            , before: function (obj) {
                //预读本地文件示例，不支持ie8
                //obj.preview(function (index, file, result) {
                //    $('#demo1').attr('src', result); //图片链接（base64）
                //});
            }
            , done: function (res) {


                $('#aburl').val(res.Result.path);

                layer.msg("上传成功");
                //如果上传失败
               // console.log(JSON.stringify(res));    
            }
            , error: function () {
                //演示失败状态，并实现重传
                var demoText = $('#demoText');
                demoText.html('<span style="color: #FF5722;">上传失败</span> <a class="layui-btn layui-btn-mini demo-reload">重试</a>');
                demoText.find('.demo-reload').on('click', function () {
                    uploadInst.upload();
                });
            }
        });


         //视频上传
        var uploadvidelInst = upload.render({
            elem: '#btnuploadvideo'
            , url: common.renderUrl('/QiniuClient/CreateDir', {
                modelID: '@ViewBag.ModleID'
            }) 
            , field: 'modelthumb'
            , accept:'video'
           
            , before: function (obj) {
                //预读本地文件示例，不支持ie8
                //obj.preview(function (index, file, result) {
                //    $('#demo1').attr('src', result); //图片链接（base64）
                //});
            }
            , done: function (res) {


                $('#videoUrl').val(res.Result.path);
                var demoText = $('#videoDemo');
                demoText.html("视频上传成功;地址为:" + res.Result.path);
                layer.msg("上传视频成功");
                //如果上传失败
               // console.log(JSON.stringify(res));    
            }
            , error: function () {
                //演示失败状态，并实现重传
                var demoText = $('#videoDemo');
                demoText.html('<span style="color: #FF5722;">上传失败</span> <a class="layui-btn layui-btn-mini demo-reload">重试</a>');
                demoText.find('.demo-reload').on('click', function () {
                    uploadvidelInst.upload();
                });
            }
        });


      //视频上传
        var uploadanswerInst = upload.render({
            elem: '#btnanswer'
            , url: common.renderUrl('/QiniuClient/CreateDir', {
                modelID: '@ViewBag.ModleID'
            })
            , field: 'modelthumb'
           
            , before: function (obj) {
                //预读本地文件示例，不支持ie8
                obj.preview(function (index, file, result) {
                    $('#answerload').attr('src', result); //图片链接（base64）
                });
            }
            , done: function (res) {

                $('#AnswerPicName').val(res.Result.path)
               // $('#videoUrl').val(res.Result.url);

                layer.msg("上传成功");
                //如果上传失败
               // console.log(JSON.stringify(res));
            }
            , error: function () {
                //演示失败状态，并实现重传
                var demoText = $('#demoText');
                demoText.html('<span style="color: #FF5722;">上传失败</span> <a class="layui-btn layui-btn-mini demo-reload">重试</a>');
                demoText.find('.demo-reload').on('click', function () {
                    uploadvidelInst.upload();
                });
            }
        });


        ///添加尺寸
        $('#btnaddsizeobj').click(function () {

            var count = $('#sizesmodel > li.layui-timeline-item').length; 


            var html = '<li class="layui-timeline-item">' +
                ' <i class="layui-icon layui-timeline-axis"></i>' +
                '<div class="layui-timeline-content layui-text">' +
                ' <h3 class="layui-timeline-title"><span class="tindex">' + count + ' </span><input type="hidden" value="' + count +'" class="index" />'+'</h3>'
                + '  <p>'
                + '尺寸标注：<input type="text" value="" lay-verify="required" class="layui-input-inline ObjcetName" />  '
                + '      零部件：<input type="text" value="" lay-verify="required" class="layui-input-inline sfrom" />   到   <input type="text" value="" lay-verify="required" class="layui-input-inline sto" /> 的长度为    <input type="number" class="slength" value="" />'
                + ' </p>'
                + '</div>'
                + '</li >'
            $('#listend').before(html);
            form.render();
            
        })


        
        var formdata = {

            getTypeItemdata: function () {

                var modelInfo = new Object();
                modelInfo.Name = $('#name').val();// "Test1";
                modelInfo.Type = "WuJian";// "物件";
                modelInfo.CNID = $("#cnid").val();
                modelInfo.aburl = $('#aburl').val(); //"/AssetBundles/testab";
                modelInfo.AbObjectName = $('#abObjectName').val();// "TestGo";
                modelInfo.TypeCalculation = new Object();
                modelInfo.TypeMethod = new Object();
                modelInfo.TypeTable = new Object();
                modelInfo.TypeProcess = new Object();

                ///
                var TypeItem = new Object();

                var Introduce = new Object();
                Introduce.Title = $('#introduceMenuName').val();// "铲运机介绍";
                Introduce.Info = $('#introduceinfo').val();//"这里是介绍这里是介绍这里是介绍这里是介绍这里是介绍这里是介绍这里是介绍里是介绍里是介绍这里是介绍这里是介绍";
                TypeItem.Introduce = Introduce;


                ///
                var Structs = new Object();
                Structs.Title = $('#structMenuname').val();//"铲运机介绍";
                var Struct = new Array();
                $('#structmodel > li.layui-timeline-item').each(function () {


                    if (this.id == 'comlistend')
                    {
                        return;
                    }

                    var StructObj1 = new Object();
                    StructObj1.Index = $(this).find('input.index').first().val();
                    StructObj1.Info = $(this).find('input.info').first().val();
                    StructObj1.GameObjectName = $(this).find('input.gamemame').first().val();
                    StructObj1.LeftOrRight = $(this).find('select').first().val();
                    Struct.push(StructObj1);
                })

                Structs.Struct = Struct;
                TypeItem.Structs = Structs;

                var Sizes = new Object();
                Sizes.Title = $('#SizeMenuName').val();//"尺寸";
                Size = new Array();
                $('#sizesmodel > li.layui-timeline-item').each(function (ele, index, val) {



                    if (this.id == 'listend') {
                        return;
                    }
                    var sizeIobj = new Object();
                    sizeIobj.Index = $(this).find('input.index').first().val();
                    sizeIobj.Length = $(this).find('input.slength').first().val();
                    sizeIobj.ObjcetName = $(this).find('input.ObjcetName').first().val();
                    sizeIobj.FromGoName = $(this).find('input.sfrom').first().val();//"zhongyangkuangjia";
                    sizeIobj.ToGaName = $(this).find('input.sto').first().val();// "JiaShiShi";
                    Size.push(sizeIobj);
                })

                Sizes.Size = Size;
                TypeItem.Sizes = Sizes;
                var Video = new Object();

                Video.Title = $('#videoName').val();// "铲运机测试";
                Video.Url = $('#videoUrl').val();// "http://www.html5videoplayer.net/videos/toystory.mp4";
                Video.Info = $('#videoInfo').val();// "描述测试";
                TypeItem.Video = Video;


                var Practices = new Object();
               
                Practices.PracticeTitle = $('#praticetextInfo').val();// "铲运机测试";
                Practices.TaskPicName = $('#taskPicName').val();// "http://www.html5videoplayer.net/videos/toystory.mp4";
                Practices.AnswerPicName = $('#AnswerPicName').val();// "描述测试";

                if ($('#open').parent().find('em').text() == "ON") {
                    Practices.IsShowAnswer = true;
                }
                else {
                    Practices.IsShowAnswer = false;
                }
                var Practice = new Array();


                $('#structanswermodel > li.layui-timeline-item').each(function () {


                    if (this.id == 'answercomlistend') {
                        return;
                    }

                    var StructObj1 = new Object();
                    StructObj1.Index = $(this).find('input.index').first().val();
                    StructObj1.Info = $(this).find('input.info').first().val();
                    StructObj1.GameObjectName = $(this).find('input.gamemame').first().val();
                    StructObj1.LeftOrRight = $(this).find('select').first().val();
                    Practice.push(StructObj1);
                })
                Practices.PracObjects = Practice;
                TypeItem.Practices = Practices;
                modelInfo.TypeItem = TypeItem;



                return modelInfo;
            }


        }



        $('#modeltest').click(function () {

            var modelInfo = new Object();
            modelInfo.Name = $('#name').val();// "Test1";
            modelInfo.Type = "WuJian";// "物件";
            modelInfo.CNID = $("#cnid").val();
            modelInfo.aburl = $('#aburl').val(); //"/AssetBundles/testab";
            modelInfo.AbObjectName = $('#abObjectName').val();// "TestGo";
            modelInfo.TypeCalculation = new Object();
            modelInfo.TypeMethod = new Object();
            modelInfo.TypeTable = new Object();
            modelInfo.TypeProcess = new Object();
                   
            ///
            var TypeItem = new Object();

            var Introduce = new Object();
            Introduce.Title = $('#introduceMenuName').val();// "铲运机介绍";
            Introduce.Info = $('#introduceinfo').val();//"这里是介绍这里是介绍这里是介绍这里是介绍这里是介绍这里是介绍这里是介绍里是介绍里是介绍这里是介绍这里是介绍";
            TypeItem.Introduce = Introduce;

 
            ///
            var Structs = new Object();
            Structs.Title = $('#structMenuname').val();//"铲运机介绍";
            var Struct = new Array();
            $('#structmodel > li.layui-timeline-item').each(function () {
                var StructObj1 = new Object();
                StructObj1.Index = $(this).find('input.index').first().val();
                StructObj1.Info = $(this).find('input.info').first().val();
                StructObj1.GameObjectName = $(this).find('input.gamemame').first().val();
                StructObj1.LeftOrRight = $(this).find('select').first().val();
                Struct.push(StructObj1);
            })

            Structs.Struct = Struct;
            TypeItem.Structs = Structs;

            var Sizes = new Object();
            Sizes.Title = $('#SizeMenuName').val();//"尺寸";
            Size = new Array();
            $('#sizesmodel > li.layui-timeline-item').each(function (ele, index, val) {

                var sizeIobj = new Object();
                sizeIobj.Index = $(this).find('input.index').first().val();
                sizeIobj.Length = $(this).find('input.slength').first().val(); 
                sizeIobj.ObjcetName = $(this).find('input.ObjcetName').first().val(); 
                sizeIobj.FromGoName = $(this).find('input.sfrom').first().val();//"zhongyangkuangjia";
                sizeIobj.ToGaName = $(this).find('input.sto').first().val();// "JiaShiShi";
                Size.push(sizeIobj);     
            })       
      
            Sizes.Size = Size;
            TypeItem.Sizes = Sizes;
            var Video = new Object();

            Video.Title = $('#videoName').val();// "铲运机测试";
            Video.Url = $('#videoUrl').val();// "http://www.html5videoplayer.net/videos/toystory.mp4";
            Video.Info = $('#videoInfo').val();// "描述测试";
            TypeItem.Video = Video;


            modelInfo.TypeItem = TypeItem;



            return modelInfo;
           // console.log(JSON.stringify(modelInfo));

        })
        ///添加构建交互
        $('#btnaddcompnentobj').click(function () {


            var count = $('#structmodel > li.layui-timeline-item').length; 

            var html = '<li class="layui-timeline-item">'
                + '<i class="layui-icon layui-timeline-axis"></i>'
                + '<div class="layui-timeline-content layui-text">'
                + '<h3 class="layui-timeline-title">部件:<span class="tindex">' + count + ' </span> <input type="hidden" value="' + count +'" class="index" /></h3>'
                + ' <p>'
                + '<div class="layui-form-item" pane>'
                + '<label class="layui-form-label">零部件信息</label>'
                + '<div class="layui-input-block">'
                + '<input type="text" value="" lay-verify="required" class="layui-input info" />'
                + '</div>'
                + '</div>'
                + '<div class="layui-form-item" pane>'
                + '<label class="layui-form-label">零部件名称</label>'
                + '<div class="layui-input-inline">'
                + '<input type="text" value="" lay-verify="required" class="layui-input gamemame" />'
                + '</div>'
                + '</div>'
                //+  ' <div class="layui-form-item" >'
                //+    '<label class="layui-form-label">开关</label>'
                //+    '<div class="layui-input-block">'
                //+        '<input type="checkbox" name="switch" lay-skin="switch">'
                //+   '</div>'
                //+ '</div>'
           
                + '<div class="layui-form-item">'
                + '<label class="layui-form-label">左或右:</label>'
                + '<div class="layui-input-block">'
                + '<select lay-verify="required">'
                + '<option value="" selected="">请选择位置</option>'
                + '<option value="true">左</option>'
                + '<option value="false">右</option>'
                + '</select>'
                + '</div>'
                + '</div>'

                + '</p>'
                + '</div>'
                + '</li>';
            $('#comlistend').before(html);
            form.render();

        })



        $('#btnaddanswercompnentobj').click(function () {
            var count = $('#structanswermodel > li.layui-timeline-item').length;

            var html = '<li class="layui-timeline-item">'
                + '<i class="layui-icon layui-timeline-axis"></i>'
                + '<div class="layui-timeline-content layui-text">'
                + '<h3 class="layui-timeline-title">答案内容:<span class="tindex">' + count + ' </span> <input type="hidden" value="' + count + '" class="index" /></h3>'
                + ' <p>'
                + '<div class="layui-form-item" pane>'
                + '<label class="layui-form-label">零部件信息</label>'
                + '<div class="layui-input-block">'
                + '<input type="text" value="" lay-verify="required" class="layui-input info" />'
                + '</div>'
                + '</div>'
                + '<div class="layui-form-item" pane>'
                + '<label class="layui-form-label">零部件名称</label>'
                + '<div class="layui-input-inline">'
                + '<input type="text" value="" lay-verify="required" class="layui-input gamemame" />'
                + '</div>'
                + '</div>'
                + '<div class="layui-form-item">'
                + '<label class="layui-form-label">左或右:</label>'
                + '<div class="layui-input-block">'
                + '<select lay-verify="required">'
                + '<option value="" selected="">请选择位置</option>'
                + '<option value="true">左</option>'
                + '<option value="false">右</option>'
                + '</select>'
                + '</div>'
                + '</div>'

                + '</p>'
                + '</div>'
                + '</li>';
            $('#answercomlistend').before(html);
            form.render();

        })
        //监听提交
        form.on('submit(formtypeModel)', function (data) {
            data.field.Resource = JSON.stringify( formdata.getTypeItemdata());
            $.post('/TypeItem/Save',
                data.field,
                function (result) {
                    layer.msg(result.Message);
                },
                "json");
            //layer.alert(JSON.stringify(data.field), {
            //    title: '最终的提交信息'
            //})
            return false;
        });


    })