IF DB_ID(N'PhongTroSV') IS NULL
    CREATE DATABASE PhongTroSV;
GO
USE PhongTroSV;
GO

/* =====================================================
   PHẦN A. TÀI KHOẢN & PHÂN QUYỀN
   ===================================================== */
CREATE TABLE vai_tro (
    id                NVARCHAR(450) NOT NULL PRIMARY KEY,
    ten               NVARCHAR(256) NOT NULL,
    dau_an_dong_thoi  NVARCHAR(MAX) NULL
);
CREATE UNIQUE INDEX UX_vai_tro_ten ON vai_tro(ten);

CREATE TABLE tai_khoan (
    id             NVARCHAR(450) NOT NULL PRIMARY KEY,
    ten_dang_nhap  NVARCHAR(256) NULL,
    email          NVARCHAR(256) NULL,
    mat_khau       NVARCHAR(200) NOT NULL,            -- lưu chuỗi ĐÃ BĂM
    so_dien_thoai  NVARCHAR(20)  NULL,
    ho_ten         NVARCHAR(150) NOT NULL DEFAULT N'',
    anh_dai_dien   NVARCHAR(300) NULL
);
CREATE UNIQUE INDEX UX_tai_khoan_ten_dang_nhap ON tai_khoan(ten_dang_nhap) WHERE ten_dang_nhap IS NOT NULL;
CREATE UNIQUE INDEX UX_tai_khoan_email         ON tai_khoan(email)         WHERE email IS NOT NULL;
CREATE UNIQUE INDEX UX_tai_khoan_sdt           ON tai_khoan(so_dien_thoai) WHERE so_dien_thoai IS NOT NULL;

CREATE TABLE vai_tro_nguoi_dung (
    tai_khoan_id NVARCHAR(450) NOT NULL,
    vai_tro_id   NVARCHAR(450) NOT NULL,
    CONSTRAINT PK_vai_tro_nguoi_dung PRIMARY KEY (tai_khoan_id, vai_tro_id),
    CONSTRAINT FK_vtnd_tai_khoan FOREIGN KEY (tai_khoan_id) REFERENCES tai_khoan(id) ON DELETE CASCADE,
    CONSTRAINT FK_vtnd_vai_tro   FOREIGN KEY (vai_tro_id)   REFERENCES vai_tro(id)   ON DELETE CASCADE
);
CREATE INDEX IX_vtnd_vai_tro ON vai_tro_nguoi_dung(vai_tro_id);

CREATE TABLE yeu_cau_vai_tro (
    id            INT IDENTITY PRIMARY KEY,
    vai_tro_id    NVARCHAR(450) NOT NULL,
    loai_yeu_cau  NVARCHAR(MAX) NULL,
    gia_tri       NVARCHAR(MAX) NULL,
    CONSTRAINT FK_ycvt_vai_tro FOREIGN KEY (vai_tro_id) REFERENCES vai_tro(id) ON DELETE CASCADE
);

CREATE TABLE yeu_cau_tai_khoan (
    id            INT IDENTITY PRIMARY KEY,
    tai_khoan_id  NVARCHAR(450) NOT NULL,
    loai_yeu_cau  NVARCHAR(MAX) NULL,
    gia_tri       NVARCHAR(MAX) NULL,
    CONSTRAINT FK_yctk_tai_khoan FOREIGN KEY (tai_khoan_id) REFERENCES tai_khoan(id) ON DELETE CASCADE
);
GO

/* =====================================================
   PHẦN B. DANH MỤC & HỒ SƠ
   ===================================================== */
CREATE TABLE truong_dai_hoc (
    id        INT IDENTITY PRIMARY KEY,
    ten       NVARCHAR(200) NOT NULL,
    dia_chi   NVARCHAR(300) NULL,
    vi_do     DECIMAL(9,6) NOT NULL,
    kinh_do   DECIMAL(9,6) NOT NULL
);

CREATE TABLE ho_so_sinh_vien (
    tai_khoan_id     NVARCHAR(450) NOT NULL PRIMARY KEY,
    truong_id        INT NULL,
    ma_sinh_vien     NVARCHAR(30) NULL,
    gioi_tinh        TINYINT NULL,                  -- 0 Nữ, 1 Nam, 2 Khác
    ngay_sinh        DATE NULL,
    nganh_hoc        NVARCHAR(100) NULL,
    gioi_thieu       NVARCHAR(500) NULL,
    ngan_sach_tu     DECIMAL(18,0) NULL,
    ngan_sach_den    DECIMAL(18,0) NULL,
    dang_tim_o_ghep  BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_hssv_tai_khoan FOREIGN KEY (tai_khoan_id) REFERENCES tai_khoan(id) ON DELETE CASCADE,
    CONSTRAINT FK_hssv_truong    FOREIGN KEY (truong_id)    REFERENCES truong_dai_hoc(id)
);

CREATE TABLE ho_so_chu_tro (
    tai_khoan_id   NVARCHAR(450) NOT NULL PRIMARY KEY,
    sdt_lien_he    NVARCHAR(20)  NULL,
    email_lien_he  NVARCHAR(100) NULL,
    dia_chi        NVARCHAR(300) NULL,
    CONSTRAINT FK_hsct_tai_khoan FOREIGN KEY (tai_khoan_id) REFERENCES tai_khoan(id) ON DELETE CASCADE
);

/* ---------- Sở thích & thói quen (phục vụ gợi ý ở ghép) ---------- */
CREATE TABLE so_thich (
    id   INT IDENTITY PRIMARY KEY,
    ten  NVARCHAR(80) NOT NULL UNIQUE
);
CREATE TABLE sinh_vien_so_thich (
    sinh_vien_id NVARCHAR(450) NOT NULL,
    so_thich_id  INT NOT NULL,
    PRIMARY KEY (sinh_vien_id, so_thich_id),
    FOREIGN KEY (sinh_vien_id) REFERENCES ho_so_sinh_vien(tai_khoan_id) ON DELETE CASCADE,
    FOREIGN KEY (so_thich_id)  REFERENCES so_thich(id) ON DELETE CASCADE
);

CREATE TABLE nhom_thoi_quen (
    id        INT IDENTITY PRIMARY KEY,
    ma        VARCHAR(30)  NOT NULL UNIQUE,
    ten       NVARCHAR(80) NOT NULL,
    trong_so  DECIMAL(4,2) NOT NULL DEFAULT 1.0
);
CREATE TABLE lua_chon_thoi_quen (
    id       INT IDENTITY PRIMARY KEY,
    nhom_id  INT NOT NULL,
    nhan     NVARCHAR(100) NOT NULL,
    gia_tri  INT NOT NULL DEFAULT 0,
    FOREIGN KEY (nhom_id) REFERENCES nhom_thoi_quen(id) ON DELETE CASCADE
);
CREATE TABLE sinh_vien_thoi_quen (
    sinh_vien_id NVARCHAR(450) NOT NULL,
    lua_chon_id  INT NOT NULL,
    PRIMARY KEY (sinh_vien_id, lua_chon_id),
    FOREIGN KEY (sinh_vien_id) REFERENCES ho_so_sinh_vien(tai_khoan_id) ON DELETE CASCADE,
    FOREIGN KEY (lua_chon_id)  REFERENCES lua_chon_thoi_quen(id) ON DELETE CASCADE
);

/* =====================================================
   PHẦN C. PHÒNG TRỌ (đã gộp nhà trọ + phòng trọ)
   ===================================================== */
CREATE TABLE loai_phong (
    id   INT IDENTITY PRIMARY KEY,
    ten  NVARCHAR(80) NOT NULL
);

CREATE TABLE phong_tro (
    id             INT IDENTITY PRIMARY KEY,
    chu_tro_id     NVARCHAR(450) NOT NULL,
    ten_nha_tro    NVARCHAR(150) NOT NULL,
    dia_chi        NVARCHAR(300) NOT NULL,
    loai_phong_id  INT NOT NULL,
    mo_ta          NVARCHAR(MAX) NULL,
    dien_tich      DECIMAL(6,1) NOT NULL,           -- m2
    gia_thue       DECIMAL(18,0) NOT NULL,
    gia_dien       DECIMAL(18,0) NULL,              -- đ/kWh
    gia_nuoc       DECIMAL(18,0) NULL,              -- đ/m3 hoặc đ/người
    tien_coc       DECIMAL(18,0) NULL,
    con_phong      BIT NOT NULL DEFAULT 1,          -- 1 còn trống, 0 hết phòng
    tim_o_ghep     BIT NOT NULL DEFAULT 0,          -- phòng đang tìm người ở ghép
    hinh_anh       NVARCHAR(300) NULL,              -- đường dẫn ảnh
    CONSTRAINT FK_phong_chu_tro FOREIGN KEY (chu_tro_id)    REFERENCES tai_khoan(id),
    CONSTRAINT FK_phong_loai    FOREIGN KEY (loai_phong_id) REFERENCES loai_phong(id),
    CONSTRAINT CK_phong_gia     CHECK (gia_thue >= 0 AND dien_tich > 0)
);
CREATE INDEX IX_phong_chu_tro ON phong_tro(chu_tro_id);
CREATE INDEX IX_phong_gia     ON phong_tro(con_phong, gia_thue) INCLUDE (dien_tich, loai_phong_id, tim_o_ghep);

CREATE TABLE phong_yeu_thich (
    sinh_vien_id NVARCHAR(450) NOT NULL,
    phong_id     INT NOT NULL,
    ngay_luu     DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    PRIMARY KEY (sinh_vien_id, phong_id),
    FOREIGN KEY (sinh_vien_id) REFERENCES tai_khoan(id) ON DELETE CASCADE,
    FOREIGN KEY (phong_id)     REFERENCES phong_tro(id) ON DELETE CASCADE
);

/* =====================================================
   PHẦN D. GHÉP PHÒNG
   ===================================================== */
CREATE TABLE yeu_cau_o_ghep (
    id             INT IDENTITY PRIMARY KEY,
    phong_id       INT NOT NULL,
    sinh_vien_id   NVARCHAR(450) NOT NULL,
    loi_nhan       NVARCHAR(500) NULL,
    trang_thai     TINYINT NOT NULL DEFAULT 0,      -- 0 Chờ, 1 Chấp nhận, 2 Từ chối, 3 Hủy
    ngay_tao       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ngay_phan_hoi  DATETIME2 NULL,
    FOREIGN KEY (phong_id)     REFERENCES phong_tro(id) ON DELETE CASCADE,
    FOREIGN KEY (sinh_vien_id) REFERENCES tai_khoan(id),
    CONSTRAINT UQ_ycog UNIQUE (phong_id, sinh_vien_id)
);

/* =====================================================
   PHẦN E. ĐÁNH GIÁ & BÁO CÁO
   ===================================================== */
CREATE TABLE danh_gia (
    id            INT IDENTITY PRIMARY KEY,
    phong_id      INT NOT NULL,
    sinh_vien_id  NVARCHAR(450) NOT NULL,
    so_sao        TINYINT NOT NULL,
    binh_luan     NVARCHAR(1000) NULL,
    hien_thi      BIT NOT NULL DEFAULT 1,           -- Admin gỡ nội dung gian lận = 0
    ngay_tao      DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    FOREIGN KEY (phong_id)     REFERENCES phong_tro(id) ON DELETE CASCADE,
    FOREIGN KEY (sinh_vien_id) REFERENCES tai_khoan(id),
    CONSTRAINT CK_danhgia_sao CHECK (so_sao BETWEEN 1 AND 5),
    CONSTRAINT UQ_danhgia UNIQUE (phong_id, sinh_vien_id)
);

CREATE TABLE bao_cao_vi_pham (
    id                   INT IDENTITY PRIMARY KEY,
    nguoi_bao_cao_id     NVARCHAR(450) NOT NULL,
    phong_id             INT NULL,
    danh_gia_id          INT NULL,
    nguoi_bi_bao_cao_id  NVARCHAR(450) NULL,
    ly_do                TINYINT NOT NULL,          -- 0 Lừa đảo, 1 Tin giả, 2 Quấy rối, 3 Khác
    mo_ta                NVARCHAR(1000) NULL,
    trang_thai           TINYINT NOT NULL DEFAULT 0,-- 0 Mới, 1 Đang xử lý, 2 Đã xử lý, 3 Bác bỏ
    nguoi_xu_ly_id       NVARCHAR(450) NULL,
    ngay_xu_ly           DATETIME2 NULL,
    ket_qua_xu_ly        NVARCHAR(500) NULL,
    ngay_tao             DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    FOREIGN KEY (nguoi_bao_cao_id)    REFERENCES tai_khoan(id),
    FOREIGN KEY (phong_id)            REFERENCES phong_tro(id) ON DELETE SET NULL,
    FOREIGN KEY (danh_gia_id)         REFERENCES danh_gia(id),
    FOREIGN KEY (nguoi_bi_bao_cao_id) REFERENCES tai_khoan(id),
    FOREIGN KEY (nguoi_xu_ly_id)      REFERENCES tai_khoan(id)
);

/* =====================================================
   PHẦN F. QUẢNG CÁO (Admin đăng tải)
   ===================================================== */
CREATE TABLE quang_cao (
    id             INT IDENTITY PRIMARY KEY,
    nguoi_dang_id  NVARCHAR(450) NOT NULL,          -- Admin đăng
    tieu_de        NVARCHAR(200) NOT NULL,
    noi_dung       NVARCHAR(1000) NULL,
    hinh_anh       NVARCHAR(300) NULL,
    duong_dan      NVARCHAR(300) NULL,              -- link khi bấm vào quảng cáo
    vi_tri         TINYINT NOT NULL DEFAULT 0,      -- 0 Banner trang chủ, 1 Cột bên, 2 Đầu trang tìm kiếm
    ngay_bat_dau   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ngay_ket_thuc  DATETIME2 NULL,
    hien_thi       BIT NOT NULL DEFAULT 1,
    ngay_tao       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    FOREIGN KEY (nguoi_dang_id) REFERENCES tai_khoan(id)
);
GO

/* =====================================================
   PHẦN G. DỮ LIỆU KHỞI TẠO (SEED)
   ===================================================== */
INSERT INTO vai_tro (id, ten, dau_an_dong_thoi) VALUES
 (CONVERT(NVARCHAR(450), NEWID()), N'Admin',    CONVERT(NVARCHAR(100), NEWID())),
 (CONVERT(NVARCHAR(450), NEWID()), N'ChuTro',   CONVERT(NVARCHAR(100), NEWID())),
 (CONVERT(NVARCHAR(450), NEWID()), N'SinhVien', CONVERT(NVARCHAR(100), NEWID()));

INSERT INTO loai_phong (ten) VALUES
 (N'Phòng trọ'), (N'Chung cư mini'), (N'Ở cùng chủ'), (N'Nhà nguyên căn'), (N'Ký túc xá tư nhân');

INSERT INTO so_thich (ten) VALUES
 (N'Âm nhạc'), (N'Thể thao'), (N'Đọc sách'), (N'Game'), (N'Nấu ăn'),
 (N'Du lịch'), (N'Phim ảnh'), (N'Lập trình');

INSERT INTO nhom_thoi_quen (ma, ten, trong_so) VALUES
 ('SLEEP',       N'Giờ giấc sinh hoạt', 1.5),
 ('PET',         N'Thú cưng',           1.0),
 ('SMOKING',     N'Hút thuốc',          2.0),
 ('PERSONALITY', N'Tính cách',          1.0),
 ('CLEAN',       N'Mức độ gọn gàng',    1.5);

INSERT INTO lua_chon_thoi_quen (nhom_id, nhan, gia_tri)
SELECT n.id, o.nhan, o.gia_tri FROM nhom_thoi_quen n
JOIN (VALUES
 ('SLEEP',N'Ngủ sớm, dậy sớm',1), ('SLEEP',N'Bình thường',2), ('SLEEP',N'Cú đêm',3),
 ('PET',N'Không nuôi, không thích thú cưng',0), ('PET',N'Chấp nhận thú cưng',1), ('PET',N'Đang nuôi thú cưng',2),
 ('SMOKING',N'Không hút thuốc',0), ('SMOKING',N'Thỉnh thoảng',1), ('SMOKING',N'Thường xuyên',2),
 ('PERSONALITY',N'Hướng nội',1), ('PERSONALITY',N'Cân bằng',2), ('PERSONALITY',N'Hướng ngoại',3),
 ('CLEAN',N'Rất gọn gàng',1), ('CLEAN',N'Bình thường',2), ('CLEAN',N'Thoải mái',3)
) AS o(ma, nhan, gia_tri) ON n.ma = o.ma;

INSERT INTO truong_dai_hoc (ten, dia_chi, vi_do, kinh_do) VALUES
 (N'Đại học Bách khoa TP.HCM', N'268 Lý Thường Kiệt, Q.10, TP.HCM',     10.772100, 106.657900),
 (N'Đại học Cần Thơ',          N'Khu II, đường 3/2, Ninh Kiều, Cần Thơ', 10.029900, 105.768800),
 (N'Đại học Kinh tế TP.HCM',   N'59C Nguyễn Đình Chiểu, Q.3, TP.HCM',    10.782000, 106.695000);
GO

USE PhongTroSV;

INSERT INTO truong_dai_hoc (ten, dia_chi, vi_do, kinh_do) VALUES
 (N'Đại học An Giang',          N'18 Ung Văn Khiêm, phường Đông Xuyên, Long Xuyên, An Giang', 10.371700, 105.432500),
 (N'Cao đẳng nghề An Giang',    N'Long Xuyên, An Giang',                                       10.380000, 105.430000),
 (N'Cao đẳng Y tế An Giang',    N'Long Xuyên, An Giang',                                       10.375000, 105.435000);

 SELECT id, ten FROM truong_dai_hoc ORDER BY ten;