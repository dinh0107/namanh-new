using namanh.Models;

namespace namanh.Utils
{
    public static class StatusTripExtensions
    {
        public static string ToColor(this StatusTrip status)
        {
            switch (status)
            {
                case StatusTrip.Latch: return "#d1e7dd";      // Chốt – xanh lá
                case StatusTrip.Deposited: return "#cff4fc";  // Đã cọc – xanh dương
                case StatusTrip.Progress: return "rgb(13 110 253)";   // Đang chạy – vàng
                case StatusTrip.Complete: return "#198754";   // Hoàn thành – xanh lá đậm
                case StatusTrip.Debt: return "rgb(255 193 7)";       // Công nợ – cam
                case StatusTrip.Cancel: return "#dc3545";     // Hủy – đỏ
                default: return "#6c757d";                    // Mặc định – xám
            }
        }
    }
}
