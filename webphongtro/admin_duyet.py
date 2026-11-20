import streamlit as st
import pandas as pd


def show_admin_duyet():
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

    st.markdown('<div class="section-header">⚙️ Phân quyền &amp; Duyệt tài khoản (Admin)</div>', unsafe_allow_html=True)
    st.caption(f"Quản trị bởi: `{st.session_state.get('user_email', 'Admin')}`")

    if "users_db" not in st.session_state:
        st.warning("⚠️ Chưa khởi tạo cơ sở dữ liệu tài khoản.")
        return

    st.markdown("---")

    users_db = st.session_state.users_db

    # THỐNG KÊ NHANH
    total = len(users_db)
    approved = sum(1 for u in users_db.values() if u["status"] == "Đã duyệt")
    pending = sum(1 for u in users_db.values() if u["status"] == "Chờ duyệt")
    rejected = sum(1 for u in users_db.values() if u["status"] == "Từ chối")

    s1, s2, s3, s4 = st.columns(4)
    for col, num, label, color in [
        (s1, total, "Tổng tài khoản", "#003580"),
        (s2, approved, "Đã duyệt", "#155724"),
        (s3, pending, "Chờ duyệt", "#856404"),
        (s4, rejected, "Từ chối", "#721c24"),
    ]:
        with col:
            st.markdown(f"""
                <div style="background:white; border-radius:12px; padding:16px; text-align:center;
                            border:1px solid #E0EBF5; box-shadow:0 2px 8px rgba(0,0,0,0.05);">
                    <div style="font-size:28px; font-weight:800; color:{color};">{num}</div>
                    <div style="font-size:13px; color:#666; margin-top:4px;">{label}</div>
                </div>
            """, unsafe_allow_html=True)

    st.markdown("<br>", unsafe_allow_html=True)

    # BẢNG DANH SÁCH TÀI KHOẢN
    st.subheader("📋 Danh sách tài khoản hệ thống")

    rows = []
    for email, info in users_db.items():
        rows.append({
            "Email": email,
            "Họ tên": info.get("name", ""),
            "Vai trò": info.get("role", ""),
            "Trạng thái": info.get("status", ""),
        })

    df = pd.DataFrame(rows)
    st.dataframe(df, use_container_width=True, hide_index=True)

    st.markdown("---")

    # THAO TÁC DUYỆT QUYỀN
    st.subheader("🔧 Phân quyền & Duyệt tài khoản")

    # Lọc danh sách không phải Admin
    non_admin_emails = [
        email for email, info in users_db.items()
        if info.get("role") != "Admin"
    ]

    if not non_admin_emails:
        st.info("ℹ️ Chưa có tài khoản thành viên nào trong hệ thống.")
        return

    with st.container(border=True):
        selected_email = st.selectbox(
            "Chọn tài khoản để phân quyền / duyệt:",
            non_admin_emails,
            format_func=lambda e: f"{users_db[e].get('name', e)}  ({e})"
        )

        target = users_db[selected_email]

        col1, col2, col3 = st.columns(3)
        with col1:
            st.markdown("**Thông tin hiện tại**")
            st.markdown(f"👤 **Họ tên**: {target.get('name', 'N/A')}")
            st.markdown(f"📧 **Email**: `{selected_email}`")
            st.markdown(f"🎭 **Vai trò hiện tại**: `{target.get('role', 'N/A')}`")
            st.markdown(f"**Trạng thái hiện tại**: `{target.get('status', 'N/A')}`")

        with col2:
            new_role = st.selectbox(
                "Phân vai trò mới:",
                ["Khách hàng", "Chủ trọ"],
                index=0 if target.get("role") == "Khách hàng" else 1,
                key="role_select"
            )

        with col3:
            status_options = ["Đã duyệt", "Chờ duyệt", "Từ chối"]
            current_status = target.get("status", "Chờ duyệt")
            status_index = status_options.index(current_status) if current_status in status_options else 1
            new_status = st.selectbox(
                "Trạng thái duyệt:",
                status_options,
                index=status_index,
                key="status_select"
            )

        if st.button("💾 Lưu thay đổi", type="primary", use_container_width=True):
            st.session_state.users_db[selected_email]["role"] = new_role
            st.session_state.users_db[selected_email]["status"] = new_status
            st.success(
                f"✅ Đã cập nhật tài khoản **{selected_email}**: "
                f"Vai trò = **{new_role}**, Trạng thái = **{new_status}**"
            )
            st.rerun()