import streamlit as st
import pandas as pd


def show_quan_ly_phong():
    st.markdown("""
        <style>
        .section-header {
            font-size: 22px;
            font-weight: 700;
            color: #003580;
            margin-bottom: 4px;
        }
        .status-approved {
            background: #d4edda;
            color: #155724;
            padding: 2px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 600;
        }
        .status-pending {
            background: #fff3cd;
            color: #856404;
            padding: 2px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 600;
        }
        </style>
    """, unsafe_allow_html=True)

    role = st.session_state.get("user_role", "")

    if role == "Chủ trọ":
        st.markdown('<div class="section-header">🏠 Quản lý phòng &amp; Tin đăng</div>', unsafe_allow_html=True)
        st.caption("Đăng tin cho thuê phòng trọ và theo dõi trạng thái duyệt tin")
    else:
        st.markdown('<div class="section-header">🏠 Quản lý phòng &amp; Duyệt tin (Admin)</div>', unsafe_allow_html=True)
        st.caption("Xem và duyệt tất cả các tin đăng phòng trọ trong hệ thống")

    st.markdown("---")

    tab_create, tab_manage = st.tabs(["📝 Đăng tin mới", "📋 Danh sách tin đăng"])

    # TAB 1: ĐĂNG TIN MỚI
    with tab_create:
        st.subheader("Tạo tin đăng phòng trọ mới")

        with st.container(border=True):
            with st.form("create_room_form"):
                col_a, col_b = st.columns(2)

                with col_a:
                    title = st.text_input("Tiêu đề tin đăng *", placeholder="Ví dụ: Cho thuê phòng trọ khép kín Cầu Giấy")
                    district = st.selectbox("Quận/Huyện *", ["Cầu Giấy", "Hai Bà Trưng", "Đống Đa", "Thanh Xuân", "Hoàn Kiếm"])
                    address = st.text_input("Địa chỉ chi tiết *", placeholder="Số nhà, tên đường...")
                    room_type = st.selectbox("Loại phòng", ["Phòng trọ độc lập", "Ở ghép", "Chung cư mini", "Nhà nguyên căn"])

                with col_b:
                    price = st.number_input("Giá thuê (VNĐ/tháng) *", min_value=500_000, max_value=20_000_000, value=3_000_000, step=100_000, format="%d")
                    area = st.number_input("Diện tích (m²)", min_value=5, max_value=200, value=20, step=1)
                    max_people = st.number_input("Số người tối đa", min_value=1, max_value=10, value=1)
                    amenities = st.multiselect(
                        "Tiện nghi kèm theo",
                        ["Điều hòa", "Nóng lạnh", "Tủ lạnh", "Máy giặt chung", "WiFi", "Chỗ để xe", "Bếp"],
                        default=["Điều hòa", "WiFi"]
                    )

                description = st.text_area("Mô tả thêm", placeholder="Mô tả chi tiết về phòng, môi trường, lưu ý...", height=100)

                submitted = st.form_submit_button("📤 Gửi tin đăng", type="primary", use_container_width=True)
                if submitted:
                    if title and address:
                        st.success(f"✅ Đã gửi tin đăng **\"{title}\"** thành công! Vui lòng chờ Admin duyệt.")
                    else:
                        st.warning("⚠️ Vui lòng điền đầy đủ các trường bắt buộc (*).")

    # TAB 2: DANH SÁCH TIN ĐĂNG
    with tab_manage:
        st.subheader("Danh sách tin đăng" + (" của bạn" if role == "Chủ trọ" else " – Toàn hệ thống"))

        # Dữ liệu mẫu
        if role == "Chủ trọ":
            my_rooms = [
                {"Mã tin": "PT01", "Tiêu đề": "Phòng trọ cao cấp Cầu Giấy", "Quận": "Cầu Giấy", "Giá (VNĐ/tháng)": "3,200,000", "Loại": "Phòng độc lập", "Trạng thái": "✅ Đã duyệt"},
                {"Mã tin": "PT02", "Tiêu đề": "Chung cư mini Đống Đa full đồ", "Quận": "Đống Đa", "Giá (VNĐ/tháng)": "4,500,000", "Loại": "Chung cư mini", "Trạng thái": "⏳ Chờ duyệt"},
            ]
        else:
            my_rooms = [
                {"Mã tin": "PT01", "Tiêu đề": "Phòng trọ cao cấp Cầu Giấy", "Chủ trọ": "Nguyễn Văn Chủ", "Quận": "Cầu Giấy", "Giá (VNĐ/tháng)": "3,200,000", "Trạng thái": "✅ Đã duyệt"},
                {"Mã tin": "PT02", "Tiêu đề": "Chung cư mini Đống Đa full đồ", "Chủ trọ": "Nguyễn Văn Chủ", "Quận": "Đống Đa", "Giá (VNĐ/tháng)": "4,500,000", "Trạng thái": "⏳ Chờ duyệt"},
                {"Mã tin": "PT03", "Tiêu đề": "Phòng ghép Hai Bà Trưng gần BK", "Chủ trọ": "Lê Thị Lan", "Quận": "Hai Bà Trưng", "Giá (VNĐ/tháng)": "1,800,000", "Trạng thái": "⏳ Chờ duyệt"},
            ]

        df = pd.DataFrame(my_rooms)
        st.dataframe(df, use_container_width=True, hide_index=True)

        if role == "Admin":
            st.markdown("---")
            st.markdown("##### ✅ Duyệt / Từ chối tin đăng")
            col_sel, col_action = st.columns([2, 1])
            with col_sel:
                selected_id = st.selectbox("Chọn mã tin:", [r["Mã tin"] for r in my_rooms])
            with col_action:
                st.markdown("<br>", unsafe_allow_html=True)
                col_approve, col_reject = st.columns(2)
                with col_approve:
                    if st.button("✅ Duyệt", key="approve_post", use_container_width=True):
                        st.success(f"Đã duyệt tin {selected_id}")
                with col_reject:
                    if st.button("❌ Từ chối", key="reject_post", use_container_width=True):
                        st.error(f"Đã từ chối tin {selected_id}")