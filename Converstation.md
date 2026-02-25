# Bug Report: Lệch số lượng Food (Food Count Discrepancy)

**Nguyên nhân:**
1. **Logic tính toán số lượng khay (Tray Logic):**
   - Trong `GameManager.cs`, số lượng khay (`totalTray`) được tính toán dựa trên tổng số lượng thức ăn (`useFood.Count`) và trung bình mỗi khay (`_avgTray`).
   - `int totalTray = Mathf.RoundToInt(useFood.Count / _avgTray);`
   - Ví dụ: Nếu có 12 món ăn và trung bình 2 món/khay => Cần 6 khay.

2. **Giới hạn vật lý của Scene (Physical Limitation):**
   - Trong Scene Unity, mỗi `GrillStation` có một số lượng `TrayItem` con cố định (ví dụ: chỉ có 3 hoặc 4 khay).
   - Biến `_listTray` trong `GrillStation.cs` chứa danh sách các khay vật lý này.

3. **Vấn đề trong vòng lặp khởi tạo:**
   - Khi `GrillStation.OnInitGrill` chạy, nó nhận `totalTray` từ GameManager (ví dụ: 6).
   - Vòng lặp tạo dữ liệu (`remindFood`) chạy dựa trên `totalTray` (6 lần).
   - TUY NHIÊN, vòng lặp gán dữ liệu vào khay vật lý (`_listTray`) chỉ chạy theo số lượng khay thực tế có trong scene.
     `for (int i = 0; i < _listTray.Count; i++)`
   - Nếu `_listTray.Count` (ví dụ: 3) nhỏ hơn `totalTray` (ví dụ: 6), thì các khay logic thứ 3, 4, 5 sẽ KHÔNG BAO GIỜ được gán vào bất kỳ object nào trong scene -> Mất thức ăn.

**Giải thích thắc mắc về OnInitLevel:**
   - Biến `totalTray` trong `GameManager` được tính toán dựa trên **SỐ LƯỢNG THỨC ĂN** (logic toán học), **KHÔNG HỀ** đếm xem trong Scene thực tế (`GrillStation`) đang có gắn bao nhiêu object `TrayItem`.
   - Ví dụ: Game Logic đòi 10 khay (từ 20 món ăn), nhưng Scene chỉ có 6 khay (2 bếp x 3). -> Mất thức ăn của 4 khay "ảo".

**Giải pháp đề xuất:**
   - **Cách 1 (Code Safety):** Trong `GrillStation`, nếu số khay yêu cầu > số khay hiện có, ta vẫn nhận đủ thức ăn nhưng dồn chúng vào các khay đang có. (Code cần kiểm tra `_listTray.Count` và dùng `Mathf.Min` để giới hạn, đồng thời đảm bảo loop phân phối hết thức ăn).
   - **Cách 2 (Design Fix):** Đảm bảo số lượng `TrayItem` trong Scene luôn đủ thừa so với `_avgTray` tính toán.

**Nguyên nhân gây ẩn đĩa (Hidden Trays):**
   - Trong `GrillStation.cs` dòng 45: 
     `for (int i = 0; i < totalTray - 1; i++)`
   - Vòng lặp này chạy đến `totalTray - 1`. Điều này có nghĩa là nếu GameManager yêu cầu 5 khay thì GrillStation chỉ khởi tạo dữ liệu cho 4 khay.
   - Kết quả: Khay thứ 5 sẽ bị ẩn (`SetActive(false)`) dù logic chia thức ăn của GameManager đã tính toán cho nó. Các thức ăn thừa sẽ được dồn vào 4 khay đầu tiên (nếu còn chỗ) qua vòng lặp `while` bên dưới.
   - **Cách khắc phục:** Sửa điều kiện vòng lặp thành `i < totalTray` để sử dụng đủ số lượng khay được yêu cầu (và vẫn cần clamp theo số lượng vật lý như bug report trên).

**Đánh giá Logic Hoàn Thành Game (Win Condition):**
   - **Về mặt toán học:** Logic của bạn là **HỢP LÝ**. Mỗi khi Merge thành công tức là đã hoàn thành 1 bộ 3 món ăn cùng loại => Trừ 1 điểm vào tổng số loại thức ăn cần hoàn thành (`_totalFood`). Khi về 0 thì thắng.
   - **Về mặt kỹ thuật (Cần cải thiện):**
     Hiện tại bạn đang trừ trực tiếp vào biến cấu hình `_totalFood` (`[SerializeField]`).
     **Vấn đề:** Nếu người chơi muốn Replay (chơi lại màn chơi) mà không load lại Scene, biến `_totalFood` lúc này đã bằng 0. Khi gọi lại `OnInitLevel`, game sẽ không sinh ra món ăn nào nữa (vì `Take(0)`).
     **Giải pháp:**
     - Giữ nguyên `_totalFood` làm biến cấu hình (không thay đổi giá trị của nó).
     - Tạo thêm một biến private `int _currentFoodCount` để đếm ngược trong màn chơi.
     - Trong `OnInitLevel`: gán `_currentFoodCount = _totalFood;`
     - Trong `OnMinusFood`: trừ `_currentFoodCount` thay vì `_totalFood`.
