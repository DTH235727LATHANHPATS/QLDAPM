import streamlit as st


# ─────────────────────────────────────────────────────────────
#  DATABASE
# ─────────────────────────────────────────────────────────────
def init_db():
    if "users_db" not in st.session_state:
        st.session_state.users_db = {
            "ly_dth235695@student.agu.edu.vn": {
                "password": "123", "role": "Admin",
                "status": "Đã duyệt", "name": "Quản trị viên",
            },
            "chutro.nguyen@gmail.com": {
                "password": "123", "role": "Chủ trọ",
                "status": "Đã duyệt", "name": "Nguyễn Văn Chủ",
            },
            "khachhang.tran@gmail.com": {
                "password": "123", "role": "Khách hàng",
                "status": "Đã duyệt", "name": "Trần Văn Khách",
            },
        }


# ─────────────────────────────────────────────────────────────
#  CSS
# ─────────────────────────────────────────────────────────────
def load_login_css():
    st.markdown("""
<style>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800;900&display=swap');

/* ── RESET ── */
html, body, .stApp {
    font-family: 'Inter', sans-serif !important;
    margin: 0; padding: 0;
}

/* ── NỀN TRANG ── */
.stApp {
    background: linear-gradient(145deg, #060E1D 0%, #0C2248 45%, #0A4A8A 100%);
    min-height: 100vh;
}

/* ── ẨN HEADER ── */
header[data-testid="stHeader"],
#MainMenu, footer { display: none !important; }

/* ── BLOCK CONTAINER: giới hạn width, căn giữa ── */
.block-container {
    max-width: 580px !important;
    padding-left: 16px !important;
    padding-right: 16px !important;
    padding-top: 0 !important;
    padding-bottom: 0 !important;
    margin: 0 auto !important;
}

section[data-testid="stMain"] > div {
    padding-top: 48px !important;
    padding-bottom: 48px !important;
}

/* ══════════════════════════════════════
   CARD CHÍNH – Dark Glassmorphism
══════════════════════════════════════ */
div[data-testid="stVerticalBlockBorderWrapper"] {
    background: rgba(8, 20, 55, 0.78) !important;
    border: 1px solid rgba(255,255,255,0.12) !important;
    border-radius: 22px !important;
    box-shadow:
        0 8px 32px rgba(0,0,0,0.4),
        0 24px 64px rgba(0,0,0,0.5),
        inset 0 1px 0 rgba(255,255,255,0.08) !important;
    overflow: hidden !important;
    padding: 0 !important;
    backdrop-filter: blur(24px) !important;
    -webkit-backdrop-filter: blur(24px) !important;
}

/* Xóa padding nội tại của Streamlit */
div[data-testid="stVerticalBlockBorderWrapper"] > div {
    padding: 0 !important;
    gap: 0 !important;
}
div[data-testid="stVerticalBlockBorderWrapper"] > div > div {
    padding: 0 !important;
    gap: 0 !important;
}

/* ── HERO HEADER BÊN TRONG CARD ── */
.card-hero {
    background: linear-gradient(135deg, #060E1D 0%, #0D3A82 60%, #1565C0 100%);
    padding: 36px 36px 30px 36px;
    text-align: center;
    position: relative;
    overflow: hidden;
}

/* Decoration circles */
.card-hero::before {
    content: '';
    position: absolute;
    top: -60px; right: -60px;
    width: 200px; height: 200px;
    background: rgba(255,255,255,0.04);
    border-radius: 50%;
}
.card-hero::after {
    content: '';
    position: absolute;
    bottom: -40px; left: -40px;
    width: 150px; height: 150px;
    background: rgba(255,255,255,0.03);
    border-radius: 50%;
}

.ch-logo {
    font-size: 46px;
    line-height: 1;
    margin-bottom: 12px;
    display: block;
}
.ch-brand {
    font-size: 19px;
    font-weight: 800;
    color: #FFFFFF;
    letter-spacing: 3px;
    margin-bottom: 7px;
}
.ch-tagline {
    font-size: 12.5px;
    color: rgba(255,255,255,0.6);
    margin-bottom: 24px;
    line-height: 1.5;
}
.ch-stats {
    display: flex;
    justify-content: center;
    gap: 10px;
    flex-wrap: wrap;
}
.chs {
    background: rgba(255,255,255,0.09);
    border: 1px solid rgba(255,255,255,0.15);
    border-radius: 11px;
    padding: 10px 15px;
    min-width: 76px;
    backdrop-filter: blur(8px);
}
.chs-n {
    font-size: 19px;
    font-weight: 800;
    color: #56C2FF;
    line-height: 1;
}
.chs-l {
    font-size: 10px;
    color: rgba(255,255,255,0.5);
    margin-top: 4px;
    line-height: 1.3;
}

/* ── FORM HEADER ── */
.form-header {
    padding: 28px 36px 10px 36px;
    border-bottom: 1px solid rgba(255,255,255,0.08);
    margin-bottom: 0;
}
.fh-eyebrow {
    font-size: 10.5px;
    font-weight: 700;
    color: #7EC8F5;
    letter-spacing: 2.5px;
    text-transform: uppercase;
    margin-bottom: 6px;
}
.fh-title {
    font-size: 21px;
    font-weight: 800;
    color: #FFFFFF;
    margin-bottom: 4px;
}
.fh-sub {
    font-size: 13px;
    color: rgba(255,255,255,0.55);
    margin-bottom: 0;
    line-height: 1.5;
}

/* ── TABS ── */
div[data-testid="stTabs"] [data-baseweb="tab-list"] {
    background: rgba(255,255,255,0.07) !important;
    border-radius: 10px !important;
    padding: 4px !important;
    gap: 2px !important;
    border: 1px solid rgba(255,255,255,0.1);
}
button[data-baseweb="tab"] {
    background: transparent !important;
    border-radius: 8px !important;
    color: rgba(255,255,255,0.75) !important;
    font-weight: 600 !important;
    font-size: 13px !important;
    padding: 8px 20px !important;
    transition: all 0.2s !important;
}
button[data-baseweb="tab"][aria-selected="true"] {
    background: rgba(255,255,255,0.14) !important;
    color: #FFFFFF !important;
    box-shadow: 0 1px 4px rgba(0,0,0,0.25) !important;
}
div[data-baseweb="tab-highlight"],
div[data-baseweb="tab-border"] { display: none !important; }

/* Padding cho tab container */
div[data-testid="stVerticalBlockBorderWrapper"] [data-testid="stTabs"] {
    padding: 20px 28px 28px 28px !important;
}

/* ── INPUTS ── */
.stTextInput label, .stSelectbox label,
.stNumberInput label, .stSlider label {
    font-size: 13px !important;
    font-weight: 600 !important;
    color: rgba(255,255,255,0.88) !important;
    margin-bottom: 2px !important;
}
div[data-baseweb="input"] {
    background: rgba(255,255,255,0.08) !important;
    border: 1.5px solid rgba(255,255,255,0.18) !important;
    border-radius: 10px !important;
    transition: border 0.2s, box-shadow 0.2s !important;
}
div[data-baseweb="input"]:focus-within {
    border-color: #5BC8FF !important;
    background: rgba(255,255,255,0.12) !important;
    box-shadow: 0 0 0 3px rgba(91,200,255,0.18) !important;
}
div[data-baseweb="input"] input {
    color: #FFFFFF !important;
    font-size: 14px !important;
    font-family: 'Inter', sans-serif !important;
    padding: 11px 14px !important;
}
div[data-baseweb="input"] input::placeholder { color: rgba(255,255,255,0.35) !important; }

div[data-baseweb="select"] > div {
    background: rgba(255,255,255,0.08) !important;
    border: 1.5px solid rgba(255,255,255,0.18) !important;
    border-radius: 10px !important;
    color: #FFFFFF !important;
}

/* ── CHECKBOX ── */
div[data-testid="stCheckbox"] label p {
    color: rgba(255,255,255,0.8) !important;
    font-size: 13px !important;
}

/* ── NÚT SUBMIT ── */
div[data-testid="stFormSubmitButton"] button {
    background: linear-gradient(135deg, #1565C0 0%, #0A3368 100%) !important;
    color: #FFFFFF !important;
    border: none !important;
    border-radius: 10px !important;
    height: 50px !important;
    font-size: 15px !important;
    font-weight: 700 !important;
    letter-spacing: 0.5px;
    width: 100%;
    box-shadow: 0 4px 16px rgba(21,101,192,0.45) !important;
    transition: all 0.25s ease !important;
}
div[data-testid="stFormSubmitButton"] button:hover {
    transform: translateY(-2px) !important;
    box-shadow: 0 8px 28px rgba(21,101,192,0.6) !important;
}
div[data-testid="stFormSubmitButton"] button p {
    color: #FFFFFF !important;
    font-weight: 700 !important;
}

/* ── NÚT THƯỜNG ── */
div.stButton > button {
    background: rgba(255,255,255,0.1) !important;
    color: #FFFFFF !important;
    border: 1.5px solid rgba(255,255,255,0.2) !important;
    border-radius: 10px !important;
    font-weight: 600 !important;
    font-size: 13px !important;
    height: 42px !important;
    transition: all 0.2s !important;
}
div.stButton > button:hover {
    background: rgba(255,255,255,0.18) !important;
    border-color: rgba(255,255,255,0.35) !important;
}

/* ── EXPANDER ── */
div[data-testid="stExpander"] {
    background: rgba(255,255,255,0.06) !important;
    border: 1.5px solid rgba(255,255,255,0.14) !important;
    border-radius: 10px !important;
}
div[data-testid="stExpander"] summary {
    color: #7EC8F5 !important;
    font-weight: 600 !important;
    font-size: 13px !important;
}

/* ── ALERT ── */
div[data-testid="stAlert"] {
    border-radius: 10px !important;
    font-size: 13px !important;
}

/* ── CAPTION ── */
div[data-testid="stCaptionContainer"] p {
    color: rgba(255,255,255,0.5) !important;
    font-size: 13px !important;
}
.stMarkdown h3 {
    color: #FFFFFF !important;
    font-size: 18px !important;
    font-weight: 700 !important;
}

/* ── DEMO BOX ── */
.demo-box {
    background: #EFF6FF;
    border: 1px solid #BFDBFE;
    border-radius: 10px;
    padding: 14px 16px;
    font-size: 12px;
    color: #1E40AF;
    line-height: 1.9;
    margin-top: 14px;
}
.demo-box strong { font-size: 12.5px; }
.demo-box code {
    background: #DBEAFE;
    padding: 1px 6px;
    border-radius: 4px;
    font-size: 11.5px;
    font-family: monospace;
}

/* ── FOOTER ── */
.page-footer {
    text-align: center;
    color: rgba(255,255,255,0.28);
    font-size: 12px;
    margin-top: 22px;
    line-height: 1.6;
}

/* ── FORM nội tại: bỏ border mặc định ── */
div[data-testid="stForm"] {
    border: none !important;
    padding: 0 !important;
}
</style>
""", unsafe_allow_html=True)


# ─────────────────────────────────────────────────────────────
#  MAIN
# ─────────────────────────────────────────────────────────────
def show_dang_nhap():
    init_db()
    load_login_css()

    # ══ CARD CHÍNH ══
    with st.container(border=True):

        # 1. HERO HEADER (1 markdown duy nhất → luôn render đúng)
        st.markdown("""
<div class="card-hero">
  <span class="ch-logo">🏠</span>
  <div class="ch-brand">STUDENT HOUSING</div>
  <div class="ch-tagline">Nền tảng tìm kiếm &amp; đặt phòng trọ dành cho sinh viên</div>
  <div class="ch-stats">
    <div class="chs"><div class="chs-n">248+</div><div class="chs-l">Phòng đang thuê</div></div>
    <div class="chs"><div class="chs-n">1.2K</div><div class="chs-l">Sinh viên dùng app</div></div>
    <div class="chs"><div class="chs-n">36</div><div class="chs-l">Chủ trọ uy tín</div></div>
    <div class="chs"><div class="chs-n">95%</div><div class="chs-l">Hài lòng</div></div>
  </div>
</div>
""", unsafe_allow_html=True)

        # 2. FORM HEADER
        st.markdown("""
<div class="form-header">
  <div class="fh-eyebrow">Chào mừng trở lại</div>
  <div class="fh-title">Đăng nhập hệ thống</div>
  <div class="fh-sub">Nhập thông tin tài khoản để tiếp tục sử dụng dịch vụ</div>
</div>
""", unsafe_allow_html=True)

        # 3. TABS
        tab_login, tab_register = st.tabs(["🔑  Đăng nhập", "📋  Đăng ký"])

        # ── TAB ĐĂNG NHẬP ──
        with tab_login:
            st.markdown("<br>", unsafe_allow_html=True)

            with st.form("form_dang_nhap", border=False):
                email = st.text_input(
                    "📧  Địa chỉ Email",
                    placeholder="example@gmail.com",
                )
                password = st.text_input(
                    "🔒  Mật khẩu",
                    type="password",
                    placeholder="Nhập mật khẩu của bạn",
                )
                remember = st.checkbox("Ghi nhớ đăng nhập")
                st.markdown("<br>", unsafe_allow_html=True)

                submitted = st.form_submit_button(
                    "→  ĐĂNG NHẬP",
                    use_container_width=True,
                )

                if submitted:
                    users = st.session_state.users_db
                    if not email or not password:
                        st.warning("⚠️  Vui lòng nhập đầy đủ email và mật khẩu.")
                    elif email not in users:
                        st.error("❌  Tài khoản không tồn tại trong hệ thống.")
                    elif users[email]["password"] != password:
                        st.error("❌  Mật khẩu không chính xác. Vui lòng thử lại.")
                    else:
                        user = users[email]
                        if user["status"] != "Đã duyệt":
                            st.warning("⏳  Tài khoản đang chờ quản trị viên phê duyệt.")
                        else:
                            st.session_state.logged_in = True
                            st.session_state.user_email = email
                            st.session_state.user_role = user["role"]
                            st.session_state.user_name = user.get("name", email)
                            st.session_state.remember = remember
                            st.success("✅  Đăng nhập thành công!")
                            st.rerun()

            # Quên mật khẩu
            with st.expander("🔑  Quên mật khẩu?"):
                st.caption("Nhập email đã đăng ký để kiểm tra tài khoản.")
                email_reset = st.text_input(
                    "Email đã đăng ký",
                    key="email_reset",
                    placeholder="example@gmail.com",
                )
                if st.button("Kiểm tra tài khoản", key="btn_reset", use_container_width=True):
                    if not email_reset:
                        st.warning("Vui lòng nhập email.")
                    elif email_reset in st.session_state.users_db:
                        st.info("ℹ️  Tài khoản tồn tại. Liên hệ Admin để đặt lại mật khẩu.")
                    else:
                        st.error("Email này chưa được đăng ký.")

            # Demo credentials
            st.markdown("""
<div class="demo-box">
  <strong>🧪 Tài khoản thử nghiệm:</strong><br>
  <span style="color:#374151">Admin:</span>
  <code>ly_dth235695@student.agu.edu.vn</code> &nbsp;·&nbsp; mật khẩu: <code>123</code><br>
  <span style="color:#374151">Chủ trọ:</span>
  <code>chutro.nguyen@gmail.com</code> &nbsp;·&nbsp; mật khẩu: <code>123</code><br>
  <span style="color:#374151">Khách hàng:</span>
  <code>khachhang.tran@gmail.com</code> &nbsp;·&nbsp; mật khẩu: <code>123</code>
</div>
""", unsafe_allow_html=True)

        # ── TAB ĐĂNG KÝ ──
        with tab_register:
            st.markdown("<br>", unsafe_allow_html=True)
            st.markdown("### Đăng ký tài khoản")
            st.caption(
                "Điền đầy đủ thông tin để đăng ký tài khoản. "
                "Tài khoản sẽ được Admin phê duyệt trước khi sử dụng."
            )
            st.markdown("<br>", unsafe_allow_html=True)

            with st.form("form_dang_ky", border=False):
                reg_name = st.text_input("👤  Họ và tên", placeholder="Nguyễn Văn A")
                reg_email = st.text_input("📧  Địa chỉ Email", placeholder="example@gmail.com")

                col_p1, col_p2 = st.columns(2)
                with col_p1:
                    reg_pass = st.text_input(
                        "🔒  Mật khẩu", type="password", placeholder="Tạo mật khẩu"
                    )
                with col_p2:
                    reg_confirm = st.text_input(
                        "🔒  Xác nhận", type="password", placeholder="Nhập lại"
                    )

                reg_role = st.selectbox(
                    "🎭  Vai trò đăng ký",
                    ["Khách hàng  –  Sinh viên tìm phòng", "Chủ trọ  –  Cho thuê phòng"],
                )
                st.markdown("<br>", unsafe_allow_html=True)

                reg_submitted = st.form_submit_button(
                    "→  ĐĂNG KÝ", use_container_width=True
                )

                if reg_submitted:
                    actual_role = "Khách hàng" if "Khách hàng" in reg_role else "Chủ trọ"
                    if not all([reg_name.strip(), reg_email.strip(), reg_pass, reg_confirm]):
                        st.warning("⚠️  Vui lòng điền đầy đủ thông tin.")
                    elif reg_email.strip() in st.session_state.users_db:
                        st.error("❌  Email này đã được đăng ký.")
                    elif reg_pass != reg_confirm:
                        st.error("❌  Mật khẩu xác nhận không khớp.")
                    elif len(reg_pass) < 3:
                        st.warning("⚠️  Mật khẩu cần ít nhất 3 ký tự.")
                    else:
                        st.session_state.users_db[reg_email.strip()] = {
                            "password": reg_pass,
                            "role": actual_role,
                            "status": "Chờ duyệt",
                            "name": reg_name.strip(),
                        }
                        st.success(
                            "✅  Đăng ký thành công! "
                            "Tài khoản đang chờ Admin phê duyệt. "
                            "Vui lòng đăng nhập sau khi được duyệt."
                        )

    # FOOTER
    st.markdown("""
<div class="page-footer">
  © 2026 Student Housing &nbsp;·&nbsp; AGU<br>
  Hệ thống quản lý &amp; tìm kiếm phòng trọ sinh viên
</div>
""", unsafe_allow_html=True)
