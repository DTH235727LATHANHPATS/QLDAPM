import streamlit as st


def show_global_css():
    """CSS toàn cục cho giao diện sau đăng nhập."""
    st.markdown("""
    <style>
    @import url('https://fonts.googleapis.com/css2?family=Be+Vietnam+Pro:wght@400;500;600;700;800&display=swap');

    .stApp {
        font-family: 'Be Vietnam Pro', sans-serif;
        background-color: #F4F7FA;
    }

    /* HEADER BANNER */
    .main-header {
        background: linear-gradient(135deg, #003580 0%, #0066CC 100%);
        padding: 36px 30px;
        border-radius: 14px;
        color: white;
        text-align: center;
        margin-bottom: 28px;
        box-shadow: 0 6px 24px rgba(0, 53, 128, 0.25);
    }

    .main-header h1 {
        font-size: 28px;
        font-weight: 800;
        margin-bottom: 8px;
        color: #ffffff;
        letter-spacing: 0.5px;
    }

    .main-header p {
        font-size: 15px;
        color: #cce0f5;
        margin: 0;
    }

    /* BADGE */
    .badge-vip {
        background-color: #febb02;
        color: #003580;
        padding: 3px 10px;
        border-radius: 20px;
        font-size: 11px;
        font-weight: 700;
        letter-spacing: 0.5px;
        display: inline-block;
    }

    .badge-type {
        background-color: #e7f0fa;
        color: #003580;
        padding: 3px 10px;
        border-radius: 20px;
        font-size: 11px;
        font-weight: 600;
        display: inline-block;
    }

    /* GIÁ TIỀN */
    .price-text {
        color: #d91b42;
        font-size: 20px;
        font-weight: 700;
        margin: 10px 0 6px 0;
    }

    /* ĐỊA CHỈ */
    .location-text {
        color: #666666;
        font-size: 13px;
        margin-bottom: 4px;
    }

    /* CARD THỐNG KÊ */
    .stat-card {
        background: white;
        border-radius: 12px;
        padding: 20px;
        text-align: center;
        border: 1px solid #E0EBF5;
        box-shadow: 0 2px 10px rgba(0,0,0,0.05);
    }

    .stat-number {
        font-size: 32px;
        font-weight: 800;
        color: #003580;
    }

    .stat-label {
        font-size: 13px;
        color: #666;
        margin-top: 4px;
    }

    /* NÚT BẤM TOÀN CỤC */
    div.stButton > button[kind="primary"] {
        background: linear-gradient(135deg, #003580, #0066CC) !important;
        color: white !important;
        border: none !important;
        border-radius: 8px !important;
        font-weight: 700 !important;
        min-height: 42px;
        transition: all 0.2s ease;
    }

    div.stButton > button[kind="primary"]:hover {
        box-shadow: 0 4px 14px rgba(0, 102, 204, 0.4) !important;
        transform: translateY(-1px);
    }

    /* FILTER SECTION */
    .filter-section {
        background: white;
        border-radius: 12px;
        padding: 16px 20px;
        margin-bottom: 20px;
        border: 1px solid #E0EBF5;
    }

    /* MATCHMAKING CARD */
    .match-percent {
        font-size: 22px;
        font-weight: 800;
        color: #0066CC;
    }
    </style>
    """, unsafe_allow_html=True)


def show_homepage():
    show_global_css()

    # 1. HEADER BANNER
    st.markdown("""
        <div class="main-header">
            <h1>🏠 Nền tảng Tìm kiếm &amp; Đặt phòng trọ Sinh viên</h1>
            <p>Hệ thống kết nối phòng trọ xác thực và đề xuất bạn ở ghép phù hợp</p>
        </div>
    """, unsafe_allow_html=True)

    # 2. THỐNG KÊ NHANH
    s1, s2, s3, s4 = st.columns(4)
    for col, num, label in [
        (s1, "248", "Phòng đang cho thuê"),
        (s2, "1.2K+", "Sinh viên đã tìm được phòng"),
        (s3, "36", "Chủ trọ uy tín"),
        (s4, "95%", "Hài lòng sau kết nối"),
    ]:
        with col:
            st.markdown(f"""
                <div class="stat-card">
                    <div class="stat-number">{num}</div>
                    <div class="stat-label">{label}</div>
                </div>
            """, unsafe_allow_html=True)

    st.markdown("<br>", unsafe_allow_html=True)

    # 3. THANH TÌM KIẾM
    st.markdown("#### 🔍 Tìm kiếm phòng trọ")
    with st.container(border=True):
        col_kw, col_dist, col_price, col_btn = st.columns([3, 2, 2, 1.5])
        with col_kw:
            st.text_input("Địa điểm", placeholder="Nhập khu vực, đường, tên trường ĐH...", label_visibility="collapsed")
        with col_dist:
            st.selectbox("Quận/Huyện", ["Tất cả Quận", "Cầu Giấy", "Hai Bà Trưng", "Đống Đa", "Thanh Xuân"], label_visibility="collapsed")
        with col_price:
            st.selectbox("Mức giá", ["Tất cả mức giá", "Dưới 2 triệu", "2 - 3.5 triệu", "Trên 3.5 triệu"], label_visibility="collapsed")
        with col_btn:
            st.button("🔍 Tìm kiếm", type="primary", use_container_width=True)

    # 4. BỘ LỌC NHANH
    st.markdown("##### Bộ lọc phổ biến")
    c1, c2, c3, c4 = st.columns(4)
    c1.button("⭐ Phòng nổi bật", use_container_width=True)
    c2.button("💰 Giá dưới 2 triệu", use_container_width=True)
    c3.button("🤝 Tìm người ở ghép", use_container_width=True)
    c4.button("🎓 Gần trường Đại học", use_container_width=True)

    st.markdown("---")

    # 5. DANH SÁCH PHÒNG TRỌ
    st.markdown("### 🏘️ Danh sách phòng trọ đề xuất")
    col1, col2, col3 = st.columns(3)

    with col1:
        with st.container(border=True):
            st.markdown('<span class="badge-vip">ĐẶC BIỆT</span> <span class="badge-type">Phòng độc lập</span>', unsafe_allow_html=True)
            st.image("https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=500", use_container_width=True)
            st.markdown("**Phòng trọ cao cấp khép kín Cầu Giấy**")
            st.markdown('<div class="location-text">📍 Cầu Giấy, Hà Nội &nbsp;|&nbsp; Gần ĐH Quốc Gia</div>', unsafe_allow_html=True)
            st.markdown('<div class="price-text">3.200.000 VNĐ / tháng</div>', unsafe_allow_html=True)
            st.caption("📐 25 m²  ·  🟢 Còn phòng")
            st.button("Xem chi tiết", key="btn_r1", type="primary", use_container_width=True)

    with col2:
        with st.container(border=True):
            st.markdown('<span class="badge-type">Ở ghép</span>', unsafe_allow_html=True)
            st.image("https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500", use_container_width=True)
            st.markdown("**Tìm 1 Nam sinh viên ở ghép – Căn hộ Bách Khoa**")
            st.markdown('<div class="location-text">📍 Hai Bà Trưng, Hà Nội &nbsp;|&nbsp; Gần ĐH Bách Khoa</div>', unsafe_allow_html=True)
            st.markdown('<div class="price-text">1.800.000 VNĐ / tháng</div>', unsafe_allow_html=True)
            st.caption("📐 35 m²  ·  👥 Đã có 2 người")
            st.button("Xem chi tiết", key="btn_r2", type="primary", use_container_width=True)

    with col3:
        with st.container(border=True):
            st.markdown('<span class="badge-vip">NỔI BẬT</span> <span class="badge-type">Chung cư mini</span>', unsafe_allow_html=True)
            st.image("https://images.unsplash.com/photo-1493809842364-78817add7ffb?w=500", use_container_width=True)
            st.markdown("**Chung cư mini đầy đủ nội thất – Đống Đa**")
            st.markdown('<div class="location-text">📍 Đống Đa, Hà Nội &nbsp;|&nbsp; Gần ĐH Ngoại Thương</div>', unsafe_allow_html=True)
            st.markdown('<div class="price-text">4.500.000 VNĐ / tháng</div>', unsafe_allow_html=True)
            st.caption("📐 30 m²  ·  🟢 Còn phòng")
            st.button("Xem chi tiết", key="btn_r3", type="primary", use_container_width=True)

    st.markdown("---")

    # 6. ĐỀ XUẤT Ở GHÉP
    st.markdown("### 🤝 Đề xuất bạn ở ghép phù hợp")
    m1, m2 = st.columns(2)

    with m1:
        with st.container(border=True):
            col_a, col_b = st.columns([3, 1])
            with col_a:
                st.markdown("#### Nguyễn Văn Nam")
                st.caption("SV Bách Khoa – K66")
            with col_b:
                st.markdown('<div class="match-percent">95%</div><div style="font-size:11px;color:#666">Phù hợp</div>', unsafe_allow_html=True)
            st.write("💰 **Ngân sách**: 1.8 – 2.2 triệu VNĐ/tháng")
            st.write("📍 **Khu vực**: Hai Bà Trưng, Đống Đa")
            st.write("🌙 **Thói quen**: Gọn gàng, không hút thuốc, thức khuya làm bài")
            st.button("📩 Gửi yêu cầu kết nối", key="m_btn1", type="primary", use_container_width=True)

    with m2:
        with st.container(border=True):
            col_a, col_b = st.columns([3, 1])
            with col_a:
                st.markdown("#### Trần Ngọc Linh")
                st.caption("SV Ngoại Thương – K65")
            with col_b:
                st.markdown('<div class="match-percent">88%</div><div style="font-size:11px;color:#666">Phù hợp</div>', unsafe_allow_html=True)
            st.write("💰 **Ngân sách**: 2.0 – 2.5 triệu VNĐ/tháng")
            st.write("📍 **Khu vực**: Đống Đa, Cầu Giấy")
            st.write("🐾 **Thói quen**: Giữ vệ sinh chung, nghỉ ngơi đúng giờ, yêu động vật")
            st.button("📩 Gửi yêu cầu kết nối", key="m_btn2", type="primary", use_container_width=True)