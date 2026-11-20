import streamlit as st
from homepage import show_homepage
from dang_nhap import show_dang_nhap
from tai_khoan import show_tai_khoan
from quan_ly_phong import show_quan_ly_phong
from admin_duyet import show_admin_duyet

# CẤU HÌNH TRANG
st.set_page_config(
    page_title="Student Housing – Hệ thống Tìm kiếm & Đặt phòng trọ Sinh viên",
    page_icon="🏠",
    layout="wide",
    initial_sidebar_state="collapsed",
)

# CSS SIDEBAR
st.markdown("""
<style>
@import url('https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@400;500;600;700;800&display=swap');

/* TOÀN CỤC */
html, body, .stApp {
    font-family: 'Be Vietnam Pro', sans-serif !important;
}

/* SIDEBAR */
[data-testid="stSidebar"] {
    background: linear-gradient(180deg, #001F4E 0%, #003580 60%, #0055A5 100%) !important;
    border-right: none !important;
}

[data-testid="stSidebar"] * {
    color: #CCDDF5 !important;
    font-family: 'Be Vietnam Pro', sans-serif !important;
}

/* LOGO SECTION */
.sidebar-logo {
    text-align: center;
    padding: 10px 0 20px 0;
    border-bottom: 1px solid rgba(255,255,255,0.15);
    margin-bottom: 16px;
}

.sidebar-logo h2 {
    color: #FFFFFF !important;
    font-size: 20px;
    font-weight: 800;
    margin: 8px 0 4px 0;
    letter-spacing: 1px;
}

.sidebar-logo p {
    color: #9BBCDC !important;
    font-size: 11px;
    margin: 0;
}

/* THÔNG TIN USER */
.user-card {
    background: rgba(255,255,255,0.08);
    border-radius: 10px;
    padding: 12px 14px;
    margin-bottom: 14px;
    border: 1px solid rgba(255,255,255,0.12);
}

.user-card .user-name {
    color: #FFFFFF !important;
    font-size: 15px;
    font-weight: 700;
}

.user-card .user-email {
    color: #9BBCDC !important;
    font-size: 11px;
    word-break: break-all;
    margin-top: 2px;
}

.user-card .role-badge {
    display: inline-block;
    background: #febb02;
    color: #001F4E !important;
    font-size: 11px;
    font-weight: 700;
    padding: 2px 10px;
    border-radius: 20px;
    margin-top: 6px;
}

/* RADIO MENU */
[data-testid="stSidebar"] .stRadio label {
    color: #CCDDF5 !important;
    font-size: 14px !important;
    font-weight: 500 !important;
}

[data-testid="stSidebar"] .stRadio label:hover {
    color: #FFFFFF !important;
}

[data-testid="stSidebar"] .stRadio > div {
    gap: 4px;
}

/* NÚT ĐĂNG XUẤT */
[data-testid="stSidebar"] div.stButton > button {
    background: rgba(255, 60, 60, 0.15) !important;
    color: #FFAAAA !important;
    border: 1px solid rgba(255, 100, 100, 0.3) !important;
    border-radius: 8px !important;
    font-weight: 600 !important;
    transition: all 0.2s;
}

[data-testid="stSidebar"] div.stButton > button:hover {
    background: rgba(255, 60, 60, 0.3) !important;
    color: #FFFFFF !important;
    border-color: rgba(255, 100, 100, 0.5) !important;
}
</style>
""", unsafe_allow_html=True)

# KHỞI TẠO BIẾN TÀI KHOẢN
if 'logged_in' not in st.session_state:
    st.session_state.logged_in = False
if 'user_role' not in st.session_state:
    st.session_state.user_role = None
if 'user_name' not in st.session_state:
    st.session_state.user_name = ""
if 'user_email' not in st.session_state:
    st.session_state.user_email = ""

# 1. NẾU CHƯA ĐĂNG NHẬP -> HIỂN THỊ MÀN HÌNH ĐĂNG NHẬP
if not st.session_state.logged_in:
    show_dang_nhap()

# 2. NẾU ĐÃ ĐĂNG NHẬP -> GIAO DIỆN CHÍNH THEO VAI TRÒ
else:
    with st.sidebar:
        # LOGO
        st.markdown("""
            <div class="sidebar-logo">
                <div style="font-size:36px;">🏠</div>
                <h2>STUDENT HOUSING</h2>
                <p>Hệ thống phòng trọ sinh viên</p>
            </div>
        """, unsafe_allow_html=True)

        # THÔNG TIN USER
        role = st.session_state.user_role
        name = st.session_state.user_name
        email = st.session_state.user_email

        st.markdown(f"""
            <div class="user-card">
                <div class="user-name">👤 {name}</div>
                <div class="user-email">{email}</div>
                <div class="role-badge">{role}</div>
            </div>
        """, unsafe_allow_html=True)

        # MENU THEO QUYỀN
        if role == "Khách hàng":
            menu_options = [
                "🏠 Trang chủ",
                "👤 Tài khoản & Tiêu chí cá nhân"
            ]
        elif role == "Chủ trọ":
            menu_options = [
                "🏠 Trang chủ",
                "🏘️ Quản lý phòng & Tin đăng"
            ]
        elif role == "Admin":
            menu_options = [
                "🏠 Trang chủ",
                "⚙️ Phân quyền & Duyệt tài khoản",
                "🏘️ Quản lý phòng & Duyệt tin"
            ]
        else:
            menu_options = ["🏠 Trang chủ"]

        st.markdown("**DANH MỤC CHỨC NĂNG**")
        menu = st.radio("Menu", menu_options, label_visibility="collapsed")

        st.markdown("<br>", unsafe_allow_html=True)
        if st.button("🚪 Đăng xuất", type="secondary", use_container_width=True):
            st.session_state.logged_in = False
            st.session_state.user_role = None
            st.session_state.user_name = ""
            st.session_state.user_email = ""
            st.rerun()

    # ĐỊNH TUYẾN TRANG
    if "Trang chủ" in menu:
        show_homepage()
    elif "Tài khoản" in menu:
        show_tai_khoan()
    elif "Quản lý phòng" in menu:
        show_quan_ly_phong()
    elif "Phân quyền" in menu:
        show_admin_duyet()