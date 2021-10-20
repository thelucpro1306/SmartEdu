var khoahoc = {
    init: function() {
        khoahoc.registerEvents();
    },
    registerEvents: function() {
        $('.abc').off('click').on('click',function(e) {
            
            e.preventDefault();
            var btn = $(this);
                var id = btn.data('id');

                $.ajax({
                    url: "/Admin/TrangThaiKhoaHoc",
                    data: { id: id },
                    dataType: "json",
                    type: "POST",
                    success: function(response) {
                        console.log(response);
                        if (response.status) {
                            btn.text("Đang mở");
                        } else {
                            btn.text("Tạm đóng");
                        }
                            

                    }
                });
            });
    }   
}
khoahoc.init();