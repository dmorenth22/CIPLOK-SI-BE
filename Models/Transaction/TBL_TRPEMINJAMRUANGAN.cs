using CIPLOK_SI_BE.Models.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CIPLOK_SI_BE.Models.Transaction
{
    public class TBL_TRPEMINJAMRUANGAN : BaseModels
    {
        [Key]
        [Required]
        [JsonPropertyName("idPeminjam")]
        public int IDPeminjam { get; set; }

        [ForeignKey("TBL_MSMAJELIS")]
        [JsonPropertyName("majelisID")]
        public int MajelisID { get; set; }

        [ForeignKey("TBL_MSACTIVITY")]
        [JsonPropertyName("activityID")]
        public int ActivityID { get; set; }

        [ForeignKey("TBL_MSROOM")]
        [JsonPropertyName("roomID")]
        public int RoomID { get; set; }

        [JsonPropertyName("isRoutine")]
        public bool IsRoutine { get; set; } = false;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("anggotaKomisi")]
        public string? AnggotaKomisi { get; set; }

        [JsonPropertyName("jenisKegiatan")]
        public string? JenisKegiatan { get; set; }

        [JsonPropertyName("majelisName")]
        public string? MajelisName { get; set; }

        [JsonPropertyName("tanggalPemakaian")]
        public DateTime? TanggalPemakaian { get; set; }

        [JsonPropertyName("roomName")]
        public string? RoomName { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; } = 1;

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; } = 0;

        public virtual TBL_MSMAJELIS? Majelis { get; set; }

        public virtual TBL_MSROOM? Room { get; set; }

        public virtual TBL_MSACTIVITY? Activity { get; set; }
    }

}
