layui.config({
    base: '../../../Content/layui/extension/js/'
}).use(['element', 'layer', 'navbar', 'tab'], function () {



    var element = layui.element
 
	$ = layui.jquery,
		layer = layui.layer,
		navbar = layui.navbar,
		tab = layui.tab({
			elem: '.layout-nav-card', //设置选项卡容器
			contextMenu:true
		});


	//iframe自适应
	$(window).on('resize', function() {
		var $content = $('.layout-nav-card .layui-tab-content');
		$content.height($(this).height() - 165);
		$content.find('iframe').each(function() {
			$(this).height($content.height());
		});
	}).resize();

	var $menu = $('#menu');
	$menu.find('li.layui-nav-item').each(function() {
		var $this = $(this);
		//绑定一级导航的点击事件
		$this.on('click', function() {
			//获取设置的模块ID
			var id = $this.find('a').data('module-id');
			//这里的数据源只是演示时用的，实际需求可能通过远程读取（根据模块ID来获取对应模块的信息）
			var url;
			switch(id) {
				case 1:
				    url = '../../../Content/layui/extension/datas/nav_content.json';
				    break;

			    case 2:
			        url = '../../../Content/layui/extension/datas/nav_product.json';
			        break;
				case 3:
				    url = '../../../Content/layui/extension/datas/nav_member.json';
				    break;
			    case 4:
			        url = '../../../Content/layui/extension/datas/nav_order.json';
			        break;
			    case 5:
			        url = '../../../Content/layui/extension/datas/nav_setting.json';
			        break;
			      
				default:
					break;
			}
			//设置数据源有两个方式。
			//第一：在此页面通过ajax读取设置  举个栗子：
			//---------这是第一个栗子----------
			/*$.getJSON('/api/xxx',{moduleId:id},function(data){
				navbar.set({
					elem: '#side',
					data: data
				});
				navbar.render();
				navbar.on('click(side)', function(data) {
					tab.tabAdd(data.field);
				});
			});*/
			//------------栗子结束--------------
			//第二：设置url
			//---------这是第二个栗子----------
			/*navbar.set({
				elem: '#side',
				url: '/api/xxx?moduleId='+id
			});
			navbar.render();
			navbar.on('click(side)', function(data) {
				tab.tabAdd(data.field);
			});*/
			//------------栗子结束--------------	

			//设置navbar
		var	enavbar = navbar({
				elem: '#side', //存在navbar数据的容器ID
				url: url
			}).render();
			//渲染navbar
		//	navbar.render();
			//监听点击事件
			enavbar.on('click(side)', function (data) {
				//layer.msg(data.field.href);
				tab.tabAdd(data.field);
			});
		});

	});
	//模拟点击内容管理
	$('.beg-layout-menu').find('a[data-module-id=1]').click();

	element.on('nav(user)', function(data) {
		var $a = data.children('a');
		if($a.data('tab') !== undefined && $a.data('tab')) {
			tab.tabAdd({
				title: $a.children('cite').text(),
				//icon: 'fa-user',
				href: $a.data('url')
			});
		}
	});

	$('.beg-layout-side-toggle').on('click', function() {
		var sideWidth = $('.beg-layout-side').width();
		if(sideWidth === 200) {
			$('.beg-layout-body').animate({
				left: '0'
			});
			$('.beg-layout-footer').animate({
				left: '0'
			});
			$('.beg-layout-side').animate({
				width: '0'
			});
		} else {
			$('.beg-layout-body').animate({
				left: '200px'
			});
			$('.beg-layout-footer').animate({
				left: '200px'
			});
			$('.beg-layout-side').animate({
				width: '200px'
			});
		}
	});
});