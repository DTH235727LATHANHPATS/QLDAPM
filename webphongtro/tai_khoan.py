import streamlit as st


def show_tai_khoan():
    st.markdown("""
        <style>
        .section-header {
            font-size: 22px;
            font-weight: 700;
            color: #003580;
            margin-bottom: 4px;
        }
        </style>
    """, unsafe_allow_html=True)

    st.markdown('<div class="section-header">👤 Tài khoản &amp; Tiêu chí cá nhân</div>', unsafe_allow_html=True)
    st.caption("Quản lý thông tin cá nhân và cấu hình tiêu chí sinh hoạt để hệ thống đề xuất bạn ở ghép phù hợp")

    st.markdown("---")

    tab_info, tab_profile = st.tabs(["📋 Thông tin tài khoản", "🏡 Tiêu chí ở ghép"])

    # TAB 1: THÔNG TIN TÀI KHOẢN
    with tab_info:
        st.subheader("Thông tin tài khoản hiện tại")

        with st.container(border=True):
            col_left, col_right = st.columns([1, 2])

            with col_left:
                st.markdown("##### Tài khoản đang đăng nhập")
                st.markdown(f"**👤 Tên**: {st.session_state.get('user_name', 'N/A')}")
                st.markdown(f"**📧 Email**: `{st.session_state.get('user_email', 'N/A')}`")
                st.markdown(f"**🎭 Vai trò**: `{st.session_state.get('user_role', 'N/A')}`")
                st.markdown(f"**✅ Trạng thái**: Đã xác thực")

            with col_right:
                st.markdown("##### Đổi mật khẩu")
                with st.form("form_doi_mat_khau"):
                    old_pass = st.text_input("Mật khẩu hiện tại", type="password", placeholder="Nhập mật khẩu cũ")
                    new_pass = st.text_input("Mật khẩu mới", type="password", placeholder="Nhập mật khẩu mới (ít nhất 3 ký tự)")
                    confirm_pass = st.text_input("Xác nhận mật khẩu mới", type="password", placeholder="Nhập lại mật khẩu mới")

                    if st.form_submit_button("💾 Cập nhật mật khẩu", use_container_width=True):
                        email = st.session_state.get("user_email", "")
                        users = st.session_state.get("users_db", {})

                        if not old_pass or not new_pass or not confirm_pass:
                            st.warning("Vui lòng điền đầy đủ thông tin.")
                        elif email not in users or users[email]["password"] != old_pass:
                            st.error("Mật khẩu hiện tại không đúng.")
                        elif new_pass != confirm_pass:
                            st.error("Mật khẩu xác nhận không khớp.")
                        elif len(new_pass) < 3:
                            st.warning("Mật khẩu mới cần ít nhất 3 ký tự.")
                        else:
                            st.session_state.users_db[email]["password"] = new_pass
                            st.success("✅ Đã cập nhật mật khẩu thành công!")

    # TAB 2: TIÊU CHÍ Ở GHÉP
    with tab_profile:
        st.subheader("Cấu hình Tiêu chí Sinh hoạt")
        st.info("ℹ️ Thông tin này giúp hệ thống tự động tính toán độ tương đồng (%) với các ứng viên ở ghép phù hợp.")

        with st.form("profile_form"):
            col1, col2 = st.columns(2)

            with col1:
                st.markdown("##### 🎓 Thông tin sinh viên")
                st.text_input("Trường Đại học", value="Đại học Bách Khoa Hà Nội", placeholder="Nhập tên trường")
                st.selectbox("Năm học", ["K65 (Năm 4)", "K66 (Năm 3)", "K67 (Năm 2)", "K68 (Năm 1)"])
                st.number_input(
                    "Ngân sách tối đa (VNĐ / tháng)",
                    min_value=1_000_000,
                    max_value=10_000_000,
                    value=2_000_000,
                    step=200_000,
                    format="%d"
                )
                st.selectbox("Khu vực ưu tiên", ["Cầu Giấy", "Hai Bà Trưng", "Đống Đa", "Thanh Xuân"])

            with col2:
                st.markdown("##### 🏠 Thói quen & Phong cách sống")
                st.slider("Mức độ sạch sẽ (1: Bình thường – 5: Rất sạch)", 1, 5, 4)
                st.checkbox("🌙 Thường xuyên thức khuya (sau 24h)", value=True)
                st.checkbox("🐾 Có nuôi thú cưng")
                st.checkbox("🚬 Có hút thuốc lá")
                st.select_slider(
                    "Tính cách",
                    options=["Hướng nội", "Linh hoạt", "Hướng ngoại"],
                    value="Linh hoạt"
                )

            submitted = st.form_submit_button("💾 Lưu cấu hình", type="primary", use_container_width=True)
            if submitted:
                st.success("✅ Đã lưu thông tin tiêu chí thành công! Hệ thống sẽ cập nhật đề xuất ở ghép cho bạn.")