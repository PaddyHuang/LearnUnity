


$(document).ready(function () {

    $("#btn-create-study").click(function () {
        var OpenUrl = controller() + "CreateStudy?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '学习经历*新增', width: '700px', height: ($(window).height() - 230), lock: true, fixed: true, drag: false, close: function () {
            bindstudy();}
        }, false);
    });

    $("#cb_all_study").click(function () {
        if ($("#cb_all_study").attr("checked")) {
            $("input[name^='cb_study']").attr("checked", true);
        } else {
            $("input[name^='cb_study']").attr("checked", false);
        }
    });

    $("#btn-del-study").click(function () {
        var CheckCount = "";
        $("input[name='cb_study']").each(function () {
            if (this.checked) {
                CheckCount += $(this).val() + ",";
            }
        });
        if (CheckCount == "") {
            art.dialog({ content: "请选择要删除的记录!", icon: 'warning', lock: true, ok: function () { this.close(); } });

        } else {
            if (CheckCount != "") {
                CheckCount = CheckCount.substring(0, CheckCount.length - 1);
            }
            art.dialog.confirm('您确定要删除所选学习经历记录？', function () {
                $.ajax({
                    type: "POST",
                    url: controller() + 'DeleteStudy/' + CheckCount,
                    success: function (data) {
                        if (data == true) {
                            var del = CheckCount.split(",");
                            for (var i = 0; i < del.length; i++) {
                                $("#tr_" + del[i]).remove();
                            }
                            art.dialog.tips("删除学习经历记录成功！");
                        }
                        else {
                            art.dialog.tips("删除学习经历记录失败，请重新操作！");
                        }
                    }
                });
            });
        }
    });

    $(".btn-update-study").live("click", function () { 
        var id = $(this).attr("id").split("-")[1];
        var OpenUrl = controller() + "CreateStudy/" + id + "?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '学习经历*修改', width: '700px', height: ($(window).height() - 230), lock: true, fixed: true, drag: false, close: function () {
            bindstudy();
        }
        }, false);
    });

    $("#cb_all_experience").click(function () {
        if ($("#cb_all_experience").attr("checked")) {
            $("input[name^='cb_experience']").attr("checked", true);
        } else {
            $("input[name^='cb_experience']").attr("checked", false);
        }
    });

    $("#btn-del-experience").click(function () {
        var CheckCount = "";
        $("input[name='cb_experience']").each(function () {
            if (this.checked) {
                CheckCount += $(this).val() + ",";
            }
        });
        if (CheckCount == "") {
            art.dialog({ content: "请选择要删除的记录!", icon: 'warning', lock: true, ok: function () { this.close(); } });

        } else {
            if (CheckCount != "") {
                CheckCount = CheckCount.substring(0, CheckCount.length - 1);
            }
            art.dialog.confirm('您确定要删除所选工作经历记录？', function () {
                $.ajax({
                    type: "POST",
                    url: controller() + 'DelExperience/' + CheckCount + "?TeacherId=" + $("#Basis_ID").val(),
                    success: function (data) {
                        if (data == true) {
                            var del = CheckCount.split(",");
                            for (var i = 0; i < del.length; i++) {
                                $("#tr_" + del[i]).remove();
                            }
                            art.dialog.tips("删除工作经历记录成功！");
                        }
                        else {
                            art.dialog.tips("删除工作经历记录失败，请重新操作！");
                        }
                    }
                });
            });
        }

    });

    $(".btn-update-experience").live("click", function () {
     
        var id = $(this).attr("id").split("-")[1];
        var OpenUrl = controller() + "Createexperience/" + id + "?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '工作经历*修改', width: '700px', height: ($(window).height() - 230), lock: true, fixed: true, drag: false, close: function () {
            bindexperience();
        }
        }, false);
    });

    $("#btn-create-experience").click(function () {
        var OpenUrl = controller() + "Createexperience?TeacherId=" + $("#Basis_ID").val();
            art.dialog.open(OpenUrl, { title: '工作经历*新增', width: '700px', height: ($(window).height() - 230), lock: true, fixed: true, drag: false, close: function () {
                bindexperience();
            }
            }, false);
        });


    $("#btn-create-degree").click(function () {
        var OpenUrl = controller() + "CreateDegree/0?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '学历及学位*新增', width: '700px', height: ($(window).height()), lock: true, fixed: true, drag: false, close: function () {
            bindDegree();
        }
        }, false);
    });

    $("#cb_all_degree").click(function () {
        if ($("#cb_all_degree").attr("checked")) {
            $("input[name^='cb_degree']").attr("checked", true);
        } else {
            $("input[name^='cb_degree']").attr("checked", false);
        }
    });

    $("#btn-del-degree").click(function () {
        var CheckCount = "";
        $("input[name='cb_degree']").each(function () {
            if (this.checked) {
                CheckCount += $(this).val() + ",";
            }
        });
        if (CheckCount == "") {
            art.dialog({ content: "请选择要删除的记录!", icon: 'warning', lock: true, ok: function () { this.close(); } });

        } else {
            if (CheckCount != "") {
                CheckCount = CheckCount.substring(0, CheckCount.length - 1);
            }
            art.dialog.confirm('您确定要删除所选学历及学位记录？', function () {
                $.ajax({
                    type: "POST",
                    url: controller() + 'DelDegree/' + CheckCount + "?TeacherId=" + $("#Basis_ID").val(),
                    success: function (data) {
                        if (data == true) {
                            var del = CheckCount.split(",");
                            for (var i = 0; i < del.length; i++) {
                                $("#tr_" + del[i]).remove();
                            }
                            art.dialog.tips("删除学历及学位记录成功！");
                        }
                        else {
                            art.dialog.tips("删除学历及学位记录失败，请重新操作！");
                        }
                    }
                });
            });
        }
    });

    $(".btn-update-degree").live("click", function () {
        var id = $(this).attr("id").split("-")[1];
        var OpenUrl = controller() + "CreateDegree/" + id + "?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '学历及学位*修改', width: '700px', height: ($(window).height()), lock: true, fixed: true, drag: false, close: function () {
            bindDegree();
        }
        }, false);
    });



    $("#btn-create-family").click(function () {
        var OpenUrl = controller() + "CreateFamily/0?TeacherId=" + $("#Basis_ID").val();
            art.dialog.open(OpenUrl, { title: '家庭成员*新增', width: '700px', height: ($(window).height() - 160), lock: true, fixed: true, drag: false, close: function () {
                bindFamily();
            }
            }, false);
        });

    $("#cb_all_family").click(function () {
        if ($("#cb_all_family").attr("checked")) {
            $("input[name^='cb_family']").attr("checked", true);
        } else {
            $("input[name^='cb_family']").attr("checked", false);
        }
    });

    $(".btn-update-family").live("click", function () {
        var id = $(this).attr("id").split("-")[1];
        var OpenUrl = controller() + "CreateFamily/" + id + "?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '家庭成员*修改', width: '700px', height: ($(window).height() - 160), lock: true, fixed: true, drag: false, close: function () {
            bindFamily();
        }
        }, false);
    });

    $("#btn-del-family").click(function () {
        var CheckCount = "";
        $("input[name='cb_family']").each(function () {
            if (this.checked) {
                CheckCount += $(this).val() + ",";
            }
        });
        if (CheckCount == "") {
            art.dialog({ content: "请选择要删除的记录!", icon: 'warning', lock: true, ok: function () { this.close(); } });

        } else {
            if (CheckCount != "") {
                CheckCount = CheckCount.substring(0, CheckCount.length - 1);
            }
            art.dialog.confirm('您确定要删除所选家庭成员记录？', function () {
                $.ajax({
                    type: "POST",
                    url: controller() + 'Delfamily/' + CheckCount + "?TeacherId=" + $("#Basis_ID").val(),
                    success: function (data) {
                        if (data == true) {
                            var del = CheckCount.split(",");
                            for (var i = 0; i < del.length; i++) {
                                $("#tr_" + del[i]).remove();
                            }
                            art.dialog.tips("删除家庭成员记录成功！");
                        }
                        else {
                            art.dialog.tips("删除家庭成员记录失败，请重新操作！");
                        }
                    }
                });
            });
        }

    });

    $("#btn-create-premium").click(function () {
        var OpenUrl = controller() + "Createpremium/0?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '奖励情况*新增', width: '700px', height: ($(window).height() - 240), lock: true, fixed: true, drag: false, close: function () {
            bindPremium();
        }
        }, false);
    });

    $("#cb_all_premium").click(function () {
        if ($("#cb_all_premium").attr("checked")) {
            $("input[name^='cb_premium']").attr("checked", true);
        } else {
            $("input[name^='cb_premium']").attr("checked", false);
        }
    });

    $(".btn-update-premium").live("click", function () {
        var id = $(this).attr("id").split("-")[1];
        var OpenUrl = controller() + "Createpremium/" + id + "?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '奖励情况*修改', width: '700px', height:($(window).height() - 240), lock: true, fixed: true, drag: false, close: function () {
            bindPremium();
        }
        }, false);
    });

    $("#btn-del-premium").click(function () {
        var CheckCount = "";
        $("input[name='cb_premium']").each(function () {
            if (this.checked) {
                CheckCount += $(this).val() + ",";
            }
        });
        if (CheckCount == "") {
            art.dialog({ content: "请选择要删除的记录!", icon: 'warning', lock: true, ok: function () { this.close(); } });

        } else {
            if (CheckCount != "") {
                CheckCount = CheckCount.substring(0, CheckCount.length - 1);
            }
            art.dialog.confirm('您确定要删除所选获奖记录？', function () {
                $.ajax({
                    type: "POST",
                    url: controller() + 'DelPremium/' + CheckCount + "?TeacherId=" + $("#Basis_ID").val(),
                    success: function (data) {
                        if (data == true) {
                            var del = CheckCount.split(",");
                            for (var i = 0; i < del.length; i++) {
                                $("#tr_" + del[i]).remove();
                            }
                            art.dialog.tips("删除获奖录成功！");
                        }
                        else {
                            art.dialog.tips("删除获奖记录失败，请重新操作！");
                        }
                    }
                });
            });
        }

    });

    $("#btn-create-punish").click(function () {
        var OpenUrl = controller() + "CreatePunish/0?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '惩罚情况*新增', width: '700px', height: ($(window).height() - 340), lock: true, fixed: true, drag: false, close: function () {
            bindPunish();
        }
        }, false);
    });

    $("#cb_all_punish").click(function () {
        if ($("#cb_all_punish").attr("checked")) {
            $("input[name^='cb_punish']").attr("checked", true);
        } else {
            $("input[name^='cb_punish']").attr("checked", false);
        }
    });

    $(".btn-update-punish").live("click", function () {
        var id = $(this).attr("id").split("-")[1];
        var OpenUrl = controller() + "CreatePunish/" + id + "?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '惩罚情况*修改', width: '700px', height: ($(window).height() - 340), lock: true, fixed: true, drag: false, close: function () {
            bindPunish();
        }
        }, false);
    });

    $("#btn-del-punish").click(function () {
        var CheckCount = "";
        $("input[name='cb_punish']").each(function () {
            if (this.checked) {
                CheckCount += $(this).val() + ",";
            }
        });
        if (CheckCount == "") {
            art.dialog({ content: "请选择要删除的记录!", icon: 'warning', lock: true, ok: function () { this.close(); } });

        } else {
            if (CheckCount != "") {
                CheckCount = CheckCount.substring(0, CheckCount.length - 1);
            }
            art.dialog.confirm('您确定要删除所选惩罚记录？', function () {
                $.ajax({
                    type: "POST",
                    url: controller() + 'DelPunish/' + CheckCount + "?TeacherId=" + $("#TeacherId").val(),
                    success: function (data) {
                        if (data == true) {
                            var del = CheckCount.split(",");
                            for (var i = 0; i < del.length; i++) {
                                $("#tr_" + del[i]).remove();
                            }
                            art.dialog.tips("删除惩罚记录成功！");
                        }
                        else {
                            art.dialog.tips("删除惩罚记录失败，请重新操作！");
                        }
                    }
                });
            });
        }

    });

    $("#btn-create-research").click(function () {
        var OpenUrl = controller() + "CreateResearchs/0?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '科研情况*新增', width: '700px', height: ($(window).height() - 160), lock: true, fixed: true, drag: false, close: function () {
            bindResearch();
        }
        }, false);
    });

    $("#cb_all_research").click(function () {
        if ($("#cb_all_research").attr("checked")) {
            $("input[name^='cb_research']").attr("checked", true);
        } else {
            $("input[name^='cb_research']").attr("checked", false);
        }
    });

    $(".btn-update-research").live("click", function () {
        var id = $(this).attr("id").split("-")[1];
        var OpenUrl = controller() + "CreateResearchs/" + id + "?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '科研情况*修改', width: '700px', height: ($(window).height() - 160), lock: true, fixed: true, drag: false, close: function () {
            bindResearch();
        }
        }, false);
    });

    $("#btn-del-research").click(function () {
        var CheckCount = "";
        $("input[name='cb_research']").each(function () {
            if (this.checked) {
                CheckCount += $(this).val() + ",";
            }
        });
        if (CheckCount == "") {
            art.dialog({ content: "请选择要删除的记录!", icon: 'warning', lock: true, ok: function () { this.close(); } });

        } else {
            if (CheckCount != "") {
                CheckCount = CheckCount.substring(0, CheckCount.length - 1);
            }
            art.dialog.confirm('您确定要删除所选科研成绩记录？', function () {
                $.ajax({
                    type: "POST",
                    url: controller() + 'Delresearchs/' + CheckCount + "?TeacherId=" + $("#Basis_ID").val(),
                    success: function (data) {
                        if (data == true) {
                            var del = CheckCount.split(",");
                            for (var i = 0; i < del.length; i++) {
                                $("#tr_" + del[i]).remove();
                            }
                            art.dialog.tips("删除科研成绩成功！");
                        }
                        else {
                            art.dialog.tips("删除科研成绩失败，请重新操作！");
                        }
                    }
                });
            });
        }

    });

    $("#btn-create-discoursea").click(function () {
        var OpenUrl = controller() + "CreateDiscoursea/0?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '论文论著*新增', width: '700px', height: ($(window).height() - 160), lock: true, fixed: true, drag: false, close: function () {
            bindDiscoursea();
        }
        }, false);
    });

    $("#cb_all_discoursea").click(function () {
        if ($("#cb_all_discoursea").attr("checked")) {
            $("input[name^='cb_discoursea']").attr("checked", true);
        } else {
            $("input[name^='cb_discoursea']").attr("checked", false);
        }
    });

    $(".btn-update-discoursea").live("click", function () {
        var id = $(this).attr("id").split("-")[1];
        var OpenUrl = controller() + "CreateDiscoursea/" + id + "?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '论文论著*修改', width: '700px', height: ($(window).height() - 160), lock: true, fixed: true, drag: false, close: function () {
            bindDiscoursea();
        }
        }, false);
    });

    $("#btn-del-discoursea").click(function () {
        var CheckCount = "";
        $("input[name='cb_discoursea']").each(function () {
            if (this.checked) {
                CheckCount += $(this).val() + ",";
            }
        });
        if (CheckCount == "") {
            art.dialog({ content: "请选择要删除的记录!", icon: 'warning', lock: true, ok: function () { this.close(); } });

        } else {
            if (CheckCount != "") {
                CheckCount = CheckCount.substring(0, CheckCount.length - 1);
            }
            art.dialog.confirm('您确定要删除所选论文论著记录？', function () {
                $.ajax({
                    type: "POST",
                    url: controller() + 'DelDiscoursea/' + CheckCount + "?TeacherId=" + $("#Basis_ID").val(),
                    success: function (data) {
                        if (data == true) {
                            var del = CheckCount.split(",");
                            for (var i = 0; i < del.length; i++) {
                                $("#tr_" + del[i]).remove();
                            }
                            art.dialog.tips("删除论文论著记录成功！");
                        }
                        else {
                            art.dialog.tips("删除论文论著记录失败，请重新操作！");
                        }
                    }
                });
            });
        }
    });

    $("#btn-create-teacherteach").click(function () {
        var OpenUrl = controller() + "CreateTeacherTeach/0?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '继续教育*新增', width: '700px', height: ($(window).height() - 180), lock: true, fixed: true, drag: false, close: function () {
            bindTeacherTeach();
        }
        }, false);
    });

    $("#cb_all_teacherteach").click(function () {
        if ($("#cb_all_teacherteach").attr("checked")) {
            $("input[name^='cb_teacherteach']").attr("checked", true);
        } else {
            $("input[name^='cb_teacherteach']").attr("checked", false);
        }
    });

    $(".btn-update-teacherteach").live("click", function () {
        var id = $(this).attr("id").split("-")[1];
        var OpenUrl = controller() + "CreateTeacherTeach/" + id + "?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '继续教育*修改', width: '700px', height: ($(window).height() - 180), lock: true, fixed: true, drag: false, close: function () {
            bindTeacherTeach();
        }
        }, false);
    });

    $("#btn-del-teacherteach").click(function () {
        var CheckCount = "";
        $("input[name='cb_teacherteach']").each(function () {
            if (this.checked) {
                CheckCount += $(this).val() + ",";
            }
        });
        if (CheckCount == "") {
            art.dialog({ content: "请选择要删除的记录!", icon: 'warning', lock: true, ok: function () { this.close(); } });

        } else {
            if (CheckCount != "") {
                CheckCount = CheckCount.substring(0, CheckCount.length - 1);
            }
            art.dialog.confirm('您确定要删除所选继续教育记录？', function () {
                $.ajax({
                    type: "POST",
                    url: controller() + 'DelTeacherTeach/' + CheckCount + "?TeacherId=" + $("#Basis_ID").val(),
                    success: function (data) {
                        if (data == true) {
                            var del = CheckCount.split(",");
                            for (var i = 0; i < del.length; i++) {
                                $("#tr_" + del[i]).remove();
                            }
                            art.dialog.tips("删除继续教育记录成功！");
                        }
                        else {
                            art.dialog.tips("删除继续教育记录失败，请重新操作！");
                        }
                    }
                });
            });
        }
    });

    $("#btn-create-certificate").click(function () {
        var OpenUrl = controller() + "CreateCertificate/0?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '证书管理*新增', width: '700px', height: ($(window).height() - 280), lock: true, fixed: true, drag: false, close: function () {
            bindCertificate();
        }
        }, false);
    });

$("#cb_all_certificate").click(function () {
    if ($("#cb_all_certificate").attr("checked")) {
        $("input[name^='cb_certificate']").attr("checked", true);
        } else {
        $("input[name^='cb_certificate']").attr("checked", false);
        }
    });

$(".btn-update-certificate").live("click", function () {
        var id = $(this).attr("id").split("-")[1];
        var OpenUrl = controller() + "CreateCertificate/" + id + "?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '证书管理*修改', width: '700px', height: ($(window).height() - 280), lock: true, fixed: true, drag: false, close: function () {
            bindCertificate();
        }
        }, false);
    });

$("#btn-del-certificate").click(function () {
        var CheckCount = "";
        $("input[name='cb_certificate']").each(function () {
            if (this.checked) {
                CheckCount += $(this).val() + ",";
            }
        });
        if (CheckCount == "") {
            art.dialog({ content: "请选择要删除的记录!", icon: 'warning', lock: true, ok: function () { this.close(); } });

        } else {
            if (CheckCount != "") {
                CheckCount = CheckCount.substring(0, CheckCount.length - 1);
            }
            art.dialog.confirm('您确定要删除所选证书记录？', function () {
                $.ajax({
                    type: "POST",
                    url: controller() + 'DelCertificate/' + CheckCount + "?TeacherId=" + $("#Basis_ID").val(),
                    success: function (data) {
                        if (data == true) {
                            var del = CheckCount.split(",");
                            for (var i = 0; i < del.length; i++) {
                                $("#tr_" + del[i]).remove();
                            }
                            art.dialog.tips("删除证书记录成功！");
                        }
                        else {
                            art.dialog.tips("删除证书记录失败，请重新操作！");
                        }
                    }
                });
            });
        }
    });


    $("#btn-create-major").click(function () {
        var OpenUrl = controller() + "CreateMajor/0?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '专业技术*新增', width: '700px', height: ($(window).height() - 80), lock: true, fixed: true, drag: false, close: function () {
            bindMajor();
        }
        }, false);
    });

    $("#cb_all_major").click(function () {
        if ($("#cb_all_major").attr("checked")) {
            $("input[name^='cb_major']").attr("checked", true);
        } else {
            $("input[name^='cb_major']").attr("checked", false);
        }
    });

    $(".btn-update-major").live("click", function () {
        var id = $(this).attr("id").split("-")[1];
        var OpenUrl = controller() + "CreateMajor/" + id + "?TeacherId=" + $("#Basis_ID").val();
        art.dialog.open(OpenUrl, { title: '专业技术*修改', width: '700px', height: ($(window).height() - 80), lock: true, fixed: true, drag: false, close: function () {
            bindMajor();
        }
        }, false);
    });

    $("#btn-del-major").click(function () {
        var CheckCount = "";
        $("input[name='cb_major']").each(function () {
            if (this.checked) {
                CheckCount += $(this).val() + ",";
            }
        });
        if (CheckCount == "") {
            art.dialog({ content: "请选择要删除的记录!", icon: 'warning', lock: true, ok: function () { this.close(); } });

        } else {
            if (CheckCount != "") {
                CheckCount = CheckCount.substring(0, CheckCount.length - 1);
            }
            art.dialog.confirm('您确定要删除所选专业技术记录？', function () {
                $.ajax({
                    type: "POST",
                    url: controller() + 'DelteMajor/' + CheckCount + "?TeacherId=" + $("#Basis_ID").val(),
                    success: function (data) {
                        if (data == true) {
                            var del = CheckCount.split(",");
                            for (var i = 0; i < del.length; i++) {
                                $("#tr_" + del[i]).remove();
                            }
                            art.dialog.tips("删除专业技术记录成功！");
                        }
                        else {
                            art.dialog.tips("删除专业技术记录失败，请重新操作！");
                        }
                    }
                });
            });
        }

    });




});

jQuery.jqtab = function (tabtit, tab_conbox, shijian) {
    $(tab_conbox).find("li").hide();
    $(tabtit).find("li:first").addClass("Remark").show();
    $(tab_conbox).find("li:first").show();
    $(tabtit).find("li").bind(shijian, function () {
        $(this).addClass("Remark").siblings("li").removeClass("Remark");
        var activeindex = $(tabtit).find("li").index(this);
        $(tab_conbox).children().eq(activeindex).show().siblings().hide();
        //                switch (activeindex) {
        //                    case 1:
        //                        bindDegree();
        //                        break;
        //                    case 2:
        //                        bindMajor();
        //                        break;
        //                    case 3:
        //                        bindexperience();
        //                        bindstudy();
        //                        break;
        //                    case 4:
        //                        bindFamily();
        //                        break;
        //                    case 5:
        //                        bindPremium();
        //                        bindPunish();
        //                        bindCertificate();
        //                        break;
        //                    case 6:
        //                        bindResearch();
        //                        bindDiscoursea();
        //                        break;
        //                    case 7:
        //                        bindTeacherTeach();
        //                        break;
        //                    case 8:
        //                        bindAssess();
        //                        break;
        //                }
        bindDegree();
        bindMajor();
        bindexperience();
        bindstudy();
        bindFamily();
        bindPremium();
        
        //bindCertificate();
        //bindResearch();
        //bindDiscoursea();
        bindTeacherTeach();
        bindAssess();
        return false;
    });

};

function bindCertificate() {
    $("#certificate_list").empty();
    var url = controller() + 'certificate/' + $("#Basis_ID").val();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            var txt = "";
            var no = 1;
            $.each(data, function (index, item) {
                txt += "<tr id=\"tr_" + item.ID + "\">";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><input  name=\"cb_certificate\" type=\"checkbox\" value=\"" + item.ID + "\" /></td>"
                }
                txt += "<td>" + no + "</td>";
                txt += "<td >" + item.CertificateName + "</td>";
                txt += "<td >" + item.CertificateDate + "</td>";
                txt += "<td >" + item.Organization + "</td><td>";
                if (item.FileString != "") {
                    txt += "<a target=\"_blank\" href=\"" + item.FileString + "\">下载附件</a>";
                }
                txt += "</td>";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><a id=\"certificate-" + item.ID + "\" class=\"btn-update-certificate\" href=\"javascript:void(0);\">[修改]</a></td>";
                }
                txt += "</tr>";
                no++;
            });
            if (no < 2) {
                txt += "<tr><td colspan=\"9\">暂无证书记录</td></tr>";
            }
            $("#certificate_list").append(txt);
        }
    }); 
}

function bindTeacherTeach() {
    $("#teacherteach_list").empty();
    var url = controller() + 'TeacherTeach/' + $("#Basis_ID").val();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            var txt = "";
            var no = 1;
            $.each(data, function (index, item) {
                txt += "<tr id=\"tr_" + item.ID + "\">";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><input  name=\"cb_teacherteach\" type=\"checkbox\" value=\"" + item.ID + "\" /></td>";
                }
                txt += "<td>" + no + "</td>";
                txt += "<td >" + item.YearName + "</td>";
                txt += "<td >" + item.CourseName + "</td>";
                txt += "<td>" + item.CourseHours + "</td>";
                txt += "<td>" + item.Complete + "</td>";
                txt += "<td >" + item.CourseCredit + "</td>";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><a id=\"teacherteach-" + item.ID + "\" class=\"btn-update-teacherteach\" href=\"javascript:void(0);\">[修改]</a></td>";
                }
                txt += "</tr>";
                no++;
            });
            if (no < 2) {
                txt += "<tr><td colspan=\"8\">暂无继续教育记录</td></tr>";
            }
            $("#teacherteach_list").append(txt);
        }
    }); 
}

function bindDiscoursea() {
    $("#discoursea_list").empty();
    var url = controller() + 'Discoursea/' + $("#Basis_ID").val();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            var txt = "";
            var no = 1;
            $.each(data, function (index, item) {
                txt += "<tr id=\"tr_" + item.ID + "\">";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><input  name=\"cb_discoursea\" type=\"checkbox\" value=\"" + item.ID + "\" /></td>";
                }
                txt += "<td>" + no + "</td>";
                txt += "<td >" + item.DiscourseName + "</td>";
                txt += "<td >" + item.DiscourseType + "</td>";
                txt += "<td >" + item.DiscourseDate + "</td>";
                txt += "<td >" + item.Publication + "</td>";
                txt += "<td>" + item.UnitName + "</td><td>";
                if (item.FileString != "") {
                    txt += "<a target=\"_blank\" href=\"" + item.FileString + "\">下载附件</a>";
                }
                txt += "</td>";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><a id=\"discoursea-" + item.ID + "\" class=\"btn-update-discoursea\" href=\"javascript:void(0);\">[修改]</a></td>";
                }
                txt += "</tr>";
                no++;
            });
            if (no < 2) {
                txt += "<tr><td colspan=\"9\">暂无论文论著记录</td></tr>";
            }
            $("#discoursea_list").append(txt);
        }
    }); 
    
}

function bindResearch() {  
    $("#research_list").empty();
    var url = controller() + 'Researchs/' + $("#Basis_ID").val();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            var txt = "";
            var no = 1;
            $.each(data, function (index, item) { 
                txt += "<tr id=\"tr_" + item.ID + "\">";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><input  name=\"cb_research\" type=\"checkbox\" value=\"" + item.ID + "\" /></td>";
                }
                txt += "<td>" + no + "</td>";
                txt += "<td >" + item.TeacherName + "</td>";
                txt += "<td >" + item.ResearchName + "</td>";
                txt += "<td >" + item.ResearchType + "</td>";
                txt += "<td >" + item.ResearchLevel + "</td>";
                txt += "<td>" + item.ResearchTime + "</td>";
                txt += "<td>" + item.Remark + "</td>";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><a id=\"research-" + item.ID + "\" class=\"btn-update-research\" href=\"javascript:void(0);\">[修改]</a></td>";
                }
                txt += "</tr>";
                no++;
            });
            if (no < 2) {
                txt += "<tr><td colspan=\"9\">暂无科研成绩记录</td></tr>";
            }
            $("#research_list").append(txt);
        }
    });    
}

function bindPunish() {
    $("#punish-list").empty();
    var url = controller() + 'punish/' + $("#Basis_ID").val();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            var txt = "";
            var no = 1;
            $.each(data, function (index, item) {
                txt += "<tr id=\"tr_" + item.ID + "\">";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><input  name=\"cb_punish\" type=\"checkbox\" value=\"" + item.ID + "\" /></td>";
                }
                txt += "<td>" + no + "</td>";
                txt += "<td >" + item.PunishDate + "</td>";
                txt += "<td>" + item.PunishName + "</td>";
                txt += "<td>" + item.UnitName + "</td>";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><a id=\"punish-" + item.ID + "\" class=\"btn-update-punish\" href=\"javascript:void(0);\">[修改]</a></td>";
                }
                txt += "</tr>";
                no++;
            });
            if (no < 2) {
                txt += "<tr><td colspan=\"7\">暂无惩罚记录</td></tr>";
            }
            $("#punish-list").append(txt);
        }
    });    
}

function bindPremium() {
    $("#premium-list").empty();
    var url = controller() + 'premium/' + $("#Basis_ID").val();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            var txt = "";
            var no = 1;
            $.each(data, function (index, item) {
                txt += "<tr id=\"tr_" + item.ID + "\">";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><input  name=\"cb_premium\" type=\"checkbox\" value=\"" + item.ID + "\" /></td>";
                }
                txt += "<td>" + no + "</td>";
                txt += "<td >" + item.PremiumDate + "</td>";
                txt += "<td>" + item.PremiumName + "</td>";
                txt += "<td>" + item.LevelName + "</td>";
                txt += "<td>" + item.UnitName + "</td><td>";
                if (item.FileString != "") {
                    txt += "<a target=\"_blank\" href=\"" + item.FileString + "\">证书附件</a>";
                }
                txt += "</td>";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><a id=\"premium-" + item.ID + "\" class=\"btn-update-premium\" href=\"javascript:void(0);\">[修改]</a></td>";
                }
                txt += "</tr>";
                no++;
            });
            if (no < 2) {
                txt += "<tr><td colspan=\"8\">暂无获奖记录</td></tr>";
            }
            $("#premium-list").append(txt);
            bindPunish();
        }
    });    
}

function bindDegree() {
    $("#degree-list").empty();
    var url = controller() + 'degree/' + $("#Basis_ID").val();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            var txt = "";
            var no = 1;
            $.each(data, function (index, item) {
                txt += "<tr id=\"tr_" + item.ID + "\">";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><input  name=\"cb_degree\" type=\"checkbox\" value=\"" + item.ID + "\" /></td>";
                }
                txt += "<td>" + no + "</td>";
                txt += "<td >" + item.Education + "</td>";
                txt += "<td >" + item.Degree + "</td>";
                txt += "<td>" + item.GraduationSchool + "</td>";
                txt += "<td>" + item.Major + "</td>";
                txt += "<td>" + item.EntranceTime + "</td>";
                txt += "<td>" + item.GraduateTime + "</td>";
                txt += "<td>" + item.LearningStyle + "</td>";
                txt += "<td>" + item.EducationCertificateCode + "</td>";
                txt += "<td>" + item.DegreeCertificateCode + "</td>";
                txt += "<td>";
                if (item.FileString != "") {
                    txt += "<a target=\"_blank\" href=\"" + item.FileString + "\">下载附件</a>";
                }
                txt += "</td>";
                txt += "<td>";
                if (item.DegreeFileString != "") {
                    txt += "<a target=\"_blank\" href=\"" + item.DegreeFileString + "\">下载附件</a>";
                }
                txt += "</td>";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><a id=\"degree-" + item.ID + "\" class=\"btn-update-degree\" href=\"javascript:void(0);\">[修改]</a></td>";
                }
                txt += "</tr>";
                no++;
            });
            if (no < 2) {
                txt += "<tr><td colspan=\"14\">暂无学历及学位记录</td></tr>";
            }
            $("#degree-list").append(txt);
        }
    });
}

function bindFamily() {
    $("#family-list").empty();
    var url = controller() + 'family/' + $("#Basis_ID").val();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            var txt = "";
            var no = 1;
            $.each(data, function (index, item) {
                txt += "<tr id=\"tr_" + item.ID + "\">";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><input  name=\"cb_family\" type=\"checkbox\" value=\"" + item.ID + "\" /></td>";
                }
                txt += "<td>" + no + "</td>";
                txt += "<td >" + item.FamilyName + "</td>";
                txt += "<td>" + item.RelationName + "</td>";
                txt += "<td>" + item.IdentityCode + "</td>";
                txt += "<td>" + item.UnitName + "</td>";
                txt += "<td>" + item.Duty + "</td>";
                txt += "<td>" + item.Phone + "</td>";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><a id=\"family-" + item.ID + "\" class=\"btn-update-family\" href=\"javascript:void(0);\">[修改]</a></td>";
                }
                txt += "</tr>";
                no++;
            });
            if (no < 2) {
                txt += "<tr><td colspan=\"9\">暂无家庭成员记录</td></tr>";
            }
            $("#family-list").append(txt);
        }
    });    
}

function bindexperience() {
    $("#experience-list").empty();
    var url = controller() + 'Experience/' + $("#Basis_ID").val();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            var txt = "";
            var no = 1;

            $.each(data, function (index, item) {
                txt += "<tr id=\"tr_" + item.ID + "\">";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><input  name=\"cb_experience\" type=\"checkbox\" value=\"" + item.ID + "\" /></td>"
                }

                txt += "<td>" + no + "</td>";
                txt += "<td >" + item.FromToDate + "</td>";

                txt += "<td >" + item.ToDate + "</td>";
                txt += "<td>" + item.Organization + "</td>";
                txt += "<td>" + item.Duty + "</td>";
                //                if (item.Reterence != undefined) {
                //                    txt += "" + item.Reterence + "";
                //                }

                txt += "<td>";
                txt += item.IsInOffice == 0 ? "否" : "是";
                txt += "</td>";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><a id=\"experience-" + item.ID + "\" class=\"btn-update-experience\" href=\"javascript:void(0);\">[修改]</a></td>";
                }
                txt += "</tr>";
                no++;
            });
            
            if (no < 2) {
                txt += "<tr><td colspan=\"8\">暂无工作经历</td></tr>";
            }
            $("#experience-list").append(txt);
        }
    });
}

function bindstudy() {
    $.ajax({
        type: "POST",
        url: controller() + 'Study/' + $("#Basis_ID").val(),
        success: function (data) {
            $("#study-list").empty();
            var txt = "";
            var no = 1;
            $.each(eval(data), function (index, item) {
                txt += "<tr id=\"tr_" + item.ID + "\">";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><input  name=\"cb_study\" type=\"checkbox\" value=\"" + item.ID + "\" /></td>"
                }
                txt += "<td>" + no + "</td>";
                txt += "<td >" + item.FromToDate + "</td>";
                txt += "<td>" + item.Organization + "</td>";
                txt += "<td>"
                if (item.Profession != undefined) {
                    txt += "" + item.Profession + "";
                }
                txt += "</td><td>";
                if (item.FileString != "") {
                    txt += "<a target=\"_blank\"  href=\"" + item.FileString + "\">证书文件</a>";
                }
                txt += "</td>";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><a id=\"study-" + item.ID + "\" class=\"btn-update-study\" href=\"javascript:void(0);\">[修改]</a></td>";
                }
                txt += "</tr>";
                no++;
            });
            if (no < 2) {
                txt += "<tr><td colspan=\"7\">暂无学校经历</td></tr>";
            }
            $("#study-list").append(txt);
        }
    });
}

function bindMajor() {
    $("#major-list").empty();
    var url = controller() + 'Major/' + $("#Basis_ID").val();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            var txt = "";
            var no = 1;
            $.each(data, function (index, item) {
                txt += "<tr id=\"tr_" + item.ID + "\">";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><input  name=\"cb_major\" type=\"checkbox\" value=\"" + item.ID + "\" /></td>";
                }
                txt += "<td>" + no + "</td>";
                txt += "<td >" + item.MajorName + "</td>";
                txt += "<td>" + item.AcquireApproach + "</td>";
                txt += "<td>" + item.AcquireTime + "</td>";
                txt += "<td>" + item.JudgeUnit + "</td>";
                txt += "<td>" + item.AppointStartTime + "</td>";
                txt += "<td>" + item.AppointEndTime + "</td>";
                txt += "<td>" + item.AppointUnit + "</td>";
                if ($("#IsOption").val() == undefined) {
                    txt += "<td><a id=\"major-" + item.ID + "\" class=\"btn-update-major\" href=\"javascript:void(0);\">[修改]</a></td>";
                }
                txt += "</tr>";
                no++;
            });
            if (no < 2) {
                txt += "<tr><td colspan=\"10\">暂无专业技术职务记录</td></tr>";
            }
            $("#major-list").append(txt);
        }
    });
}

function bindAssess() {
    $("#assess_list").empty();
    var url = controller() + 'Assess/' + $("#Basis_ID").val();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            var txt = "";
            var no = 1;
            $.each(data, function (index, item) {
                txt += "<tr id=\"tr_" + item.ID + "\">";
                txt += "<td>" + no + "</td>";
                txt += "<td >" + item.AssessTime + "</td>";
                txt += "<td>" + item.AssessResult + "</td>";
                txt += "<td>" + item.AssessUnit + "</td>";
                txt += "</tr>";
                no++;
            });
            
            if (no < 2) {
                txt += "<tr><td colspan=\"4\">暂无年度考核记录</td></tr>";
            }
            $("#assess_list").append(txt);
        }
    });
}