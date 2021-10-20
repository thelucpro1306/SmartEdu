window.dqdt = window.dqdt || {};
dqdt.init = function () {
	dqdt.showPopup();
	dqdt.hidePopup();	
};


/********************************************************
# SHOW POPUP
********************************************************/
dqdt.showPopup = function (selector) {
	$(selector).addClass('active');
};

/********************************************************
# HIDE POPUP
********************************************************/
dqdt.hidePopup = function (selector) {
	$(selector).removeClass('active');
}



/********************************************************
# Quickview
********************************************************/

initQuickView();
var product = {};
var currentLinkQuickView = '';
var option1 = '';
var option2 = '';
function setButtonNavQuickview() {
	$("#quickview-nav-button a").hide();
	$("#quickview-nav-button a").attr("data-index", "");
	var listProducts = $(currentLinkQuickView).closest(".slide").find("a.quick-view");
	if(listProducts.length > 0) {
		var currentPosition = 0;
		for(var i = 0; i < listProducts.length; i++) {
			if($(listProducts[i]).data("handle") == $(currentLinkQuickView).data("handle")) {
				currentPosition = i;
				break;
			}
		}
		if(currentPosition < listProducts.length - 1) {
			$("#quickview-nav-button .btn-next-product").show();
			$("#quickview-nav-button .btn-next-product").attr("data-index", currentPosition + 1);
		}
		if(currentPosition > 0) {
			$("#quickview-nav-button .btn-previous-product").show();
			$("#quickview-nav-button .btn-previous-product").attr("data-index", currentPosition - 1);
		}
	}
	$("#quickview-nav-button a").click(function() {
		$("#quickview-nav-button a").hide();
		var indexLink = parseInt($(this).data("index"));
		if(!isNaN(indexLink) && indexLink >= 0) {
			var listProducts = $(currentLinkQuickView).closest(".slide").find("a.quick-view");
			if(listProducts.length > 0 && indexLink < listProducts.length) {
				//$(".quickview-close").trigger("click");
				$(listProducts[indexLink]).trigger("click");
			}
		}
	});
}
function initQuickView(){
	$(document).on("click", "#thumblist_quickview li", function() {		
		changeImageQuickView($(this).find("img:first-child"), ".product-featured-image-quickview");
		$(this).parent().parent().find('li').removeClass('active');
		$(this).addClass('active');
	});
	/*
	$(document).on("click", ".btn-quickview", function(e) {
		e.preventDefault();		
		$("#quick-view-product").hide();
		var form = $(this).parents('form');			
		$.ajax({
			type: 'POST',
			url: '/cart/add.js',
			async: false,
			data: form.serialize(),
			dataType: 'json',
			error: addToCartFail,
			beforeSend: function() {  				
				if(window.theme_load == "icon"){
					dqdt.showLoading('.btn-addToCart');
				} else{
					dqdt.showPopup('.loading');
				}
			},
			success: addToCartSuccess,
			cache: false
		});

	});
	*/

	$(document).on('click', '.quick-view', function(e) {
		e.preventDefault();
		dqdt.showPopup('.loading');
		var producthandle = $(this).data("handle");
		currentLinkQuickView = $(this);
		Bizweb.getProduct(producthandle,function(product) {
			var qvhtml = $("#quickview-modal").html();
			$(".quick-view-product").html(qvhtml);
			var quickview= $(".quick-view-product");
			if(product.content != null){
				var productdes = product.content.replace(/(<([^>]+)>)/ig,"");
			}else{
				var productdes = "";
			}
			var featured_image = product.featured_image;
			if(featured_image == null){
				featured_image = 'http://bizweb.dktcdn.net/thumb/grande/assets/themes_support/noimage.gif';
			}

			// Reset current link quickview and button navigate in Quickview
			setButtonNavQuickview();
			productdes = productdes.split(" ").splice(0,60).join(" ")+"...";	
			if(featured_image != null){
				quickview.find(".view_full_size img").attr("src",featured_image);
			}


			quickview.find(".product-item").attr("id", "product-" + product.id);
			quickview.find(".qv-link").attr("href",product.url);
			quickview.find(".variants").attr("id", "product-actions-" + product.id);
			quickview.find(".variants select").attr("id", "product-select-" + product.id);

			quickview.find(".qwp-name").text(product.name);
			quickview.find(".review .shopify-product-reviews-badge").attr("data-id",product.id);
			if(product.vendor){
				quickview.find(".brand").append("<b>Thương hiệu: </b>"+product.vendor);
			}else{
				quickview.find(".brand").append("<b>Thương hiệu: </b>Không có");
			}
			if(product.product_type){
				quickview.find(".type").append("<b> </b>"+product.product_type);
			}else{
				quickview.find(".type").append("<b> </b>Không có");
			}
			quickview.find(".product-description").html(productdes);

			/*Gia san pham*/
			if(product.price < 1 && product.variants.length < 1){
				
				quickview.find(".price").html('Liên hệ');
				quickview.find("del").html('');
				quickview.find("#quick-view-product form").hide();


				quickview.find(".prices").html('<span class="price h2">Liên hệ</span>');
				quickview.find(".add_to_cart_detail .text_buy").html('Liên hệ');
				quickview.find(".add_to_cart_detail").removeClass('add_to_cart');

			}
			else{
				quickview.find("#quick-view-product form").show();
				quickview.find(".price").html(Bizweb.formatMoney(product.price, "{{amount_no_decimals_with_comma_separator}}₫" ));
			}

			
			
			if (product.compare_at_price_max > product.price) {

				quickview.find(".old-price").html(Bizweb.formatMoney(product.compare_at_price_max, "{{amount_no_decimals_with_comma_separator}}₫" )).show();
			}
			else {
				quickview.find(".old-price").html("");
				quickview.find(".sale").removeClass("sale-price")
			}
			if (!product.available) {

				quickViewVariantsSwatch(product, quickview);

				quickview.find(".add_to_cart_detail .text_buy").text("Hết hàng");
				quickview.find(".add_to_cart_detail").addClass("disabled").attr("disabled", "disabled");;
				if(product.variants.length > 1){

					quickview.find("select, .dec, .inc, .variants label").show();

				}else{
					quickview.find("select, .dec, .inc, .variants label").hide();
				}

			}
			else {
				quickViewVariantsSwatch(product, quickview);
				if(product.variants.length > 1){
					$('#quick-view-product form').show();

				}else{
					if(product.price < 1){

						$('#quick-view-product form').hide();
					}else{
						$('#quick-view-product form').show();
					}
				}
			}


			if(product.price < 1 && product.variants.length < 2){				
				quickview.find("span.prices > .price").text('Liên hệ');
				
				quickview.find("del").html('');
				quickview.find("#quick-view-product form").hide();
				quickview.find(".add_to_cart_detail span").html('Liên hệ');
			}
			else{
				quickview.find("#quick-view-product form").show();
				quickview.find(".price").html(Bizweb.formatMoney(product.price, "{{amount_no_decimals_with_comma_separator}}₫" ));
			}

			
			quickview.find('.more_info_block .page-product-heading li:first, .more_info_block .tab-content section:first').addClass('active');

			$("#quick-view-product").fadeIn(500);

			$(".view_scroll_spacer").removeClass("hidden");

			loadQuickViewSlider(product, quickview);

			//initQuickviewAddToCart();

			$(".quick-view").fadeIn(500);
			if ($(".quick-view .total-price").length > 0) {
				$(".quick-view input[name=quantity]").on("change", updatePricingQuickView)
			}			
			updatePricingQuickView();
			// Setup listeners to add/subtract from the input
			$(".js-qty__adjust").on("click", function() {
				var el = $(this),
					id = el.data("id"),
					qtySelector = el.siblings(".js-qty__num"),
					qty = parseInt(qtySelector.val().replace(/\D/g, ''));

				var qty = validateQty(qty);

				// Add or subtract from the current quantity
				if (el.hasClass("js-qty__adjust--plus")) {
					qty = qty + 1;
				} else {
					qty = qty - 1;
					if (qty <= 1) qty = 1;
				}

				// Update the input's number
				qtySelector.val(qty);
				updatePricingQuickView();
			});
			$(".js-qty__num").on("change", function() {
				updatePricingQuickView();
			});
		});
		return false;
	});
}

function loadQuickViewSlider(n, r) {
	productImage();
	var loadingImgQuickView = $('.loading-imgquickview');
	var s = Bizweb.resizeImage(n.featured_image, "grande");
	r.find(".quickview-featured-image").append('<a href="' + n.url + '"><img src="' + s + '" title="' + n.title + '"/><div style="height: 100%; width: 100%; top:0; left:0 z-index: 2000; position: absolute; display: none; background: url(' + window.loading_url + ') 50% 50% no-repeat;"></div></a>');
	if (n.images.length > 1) {
		var o = r.find(".more-view-wrapper ul");
		for (i in n.images) {
			var u = Bizweb.resizeImage(n.images[i], "grande");
			var a = Bizweb.resizeImage(n.images[i], "compact");
			var f = '<li><a href="javascript:void(0)" data-imageid="' + n.id + '"" data-zoom-image="' + u + '" ><img src="' + u + '" alt="Proimage" /></a></li>';
			o.append(f)
		}
		o.find("a").click(function() {
			var t = r.find("#product-featured-image-quickview");
			if (t.attr("src") != $(this).attr("data-image")) {
				t.attr("src", $(this).attr("data-image"));
				loadingImgQuickView.show();
				t.load(function(t) {
					loadingImgQuickView.hide();
					$(this).unbind("load");
					loadingImgQuickView.hide()
				})
			}
		});
		o.owlCarousel({
			navigation: true,
			items: 3,
			margin:10,
			itemsDesktop: [1199, 3],
			itemsDesktopSmall: [979, 3],
			itemsTablet: [768, 3],
			itemsTabletSmall: [540, 3],
			itemsMobile: [360, 3]
		}).css("visibility", "visible")
	} else {        
		r.find(".quickview-more-views").remove();
		r.find(".quickview-more-view-wrapper-jcarousel").remove()
	}
}
function quickViewVariantsSwatch(t, quickview) {

	var v = '<input type="hidden" name="id" value="' + t.id + '">';
	quickview.find("form.variants").append(v);
	if (t.variants.length > 1) {	
		for (var r = 0; r < t.variants.length; r++) {
			var i = t.variants[r];
			var s = '<option value="' + i.id + '">' + i.title + "</option>";
			quickview.find("form.variants > select").append(s)
		}
		var ps = "product-select-" + t.id;
		new Bizweb.OptionSelectors( ps, { 
			product: t, 
			onVariantSelected: selectCallbackQuickView
		});
		if (t.options.length == 1) {

			quickview.find(".selector-wrapper:eq(0)").prepend("<label>" + t.options[0].name + "</label>")

		}
		quickview.find("form.variants .selector-wrapper label").each(function(n, r) {
			$(this).html(t.options[n].name)
		})
	}
	else {
		quickview.find("form.variants > select").remove();
		var q = '<input type="hidden" name="variantId" value="' + t.variants[0].id + '">';
		quickview.find("form.variants").append(q);
	}
}
function productImage() {
	$('#thumblist').owlCarousel({
		navigation: true,
		items: 4,
		itemsDesktop: [1199, 4],
		itemsDesktopSmall: [979, 4],
		itemsTablet: [768, 4],
		itemsTabletSmall: [540, 4],
		itemsMobile: [360, 4]
	});

	if (!!$.prototype.fancybox){
		$('li:visible .fancybox, .fancybox.shown').fancybox({
			'hideOnContentClick': true,
			'openEffect'	: 'elastic',
			'closeEffect'	: 'elastic'
		});
	}
}
/* Quick View ADD TO CART */

function updatePricingQuickView() {

	//Currency.convertAll(window.shop_currency, $("#currencies a.selected").data("currency"), "span.money", "money_format")

	var regex = /([0-9]+[.|,][0-9]+[.|,][0-9]+)/g;
	var unitPriceTextMatch = jQuery('.quick-view-product .price').text().match(regex);
	if (!unitPriceTextMatch) {
		regex = /([0-9]+[.|,][0-9]+)/g;
		unitPriceTextMatch = jQuery('.quick-view-product .price').text().match(regex);
	}
	if (unitPriceTextMatch) {
		var unitPriceText = unitPriceTextMatch[0];
		var unitPrice = unitPriceText.replace(/[.|,]/g,'');
		var quantity = parseInt(jQuery('.quick-view-product input[name=quantity]').val());
		var totalPrice = unitPrice * quantity;
		var totalPriceText = Bizweb.formatMoney(totalPrice, "{{amount_no_decimals_with_comma_separator}}₫" );
		totalPriceText = totalPriceText.match(regex)[0];
		var regInput = new RegExp(unitPriceText, "g");
		var totalPriceHtml = jQuery('.quick-view-product .price').html().replace(regInput ,totalPriceText);
		jQuery('.quick-view-product .total-price span').html(totalPriceHtml);
	}
}
$(document).on('click', '.quickview-close, #overlay, .quickview-overlay, .fancybox-overlay', function(e){
	$("#quick-view-product").fadeOut(0);
	dqdt.hidePopup('.loading');
});
/*Bắt trường hợp số âm number*/
$(document).on('click','span.btn_minus_qv',function(e){
	var va = parseInt($(this).parent().find('input').val());
	if(va > 1){
		$(this).parent().find('input').val(va - 1);
	}
});

$(document).on('click','span.btn_plus_qv',function(e){
	var va = parseInt($(this).parent().find('input').val());
	$(this).parent().find('input').val(va + 1);

});