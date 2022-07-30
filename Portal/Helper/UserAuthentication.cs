using Portal.Enums;
using Portal.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Helper
{
    public static class UserAuthentication
    {
        public static bool IsControl(string UserId, string PageGuid, AuthorizationTypes AuthorizationTypes)
        {

            using (PortalDB db = new PortalDB())
            {
                var currentUserId = Convert.ToInt32(UserId); //inte çevirdik.
                var user = db.SystemUser.FirstOrDefault(x => x.ID == currentUserId); // Kullanıcıyı buluyoruz
                if (user != null)
                {
                    var page = db.Pages.FirstOrDefault(x => x.PageGuid == PageGuid); // Hangi sayfayı kontrol edeceğimizi buluyoruz
                    var auth = db.Authorizations.FirstOrDefault(x => x.UserID == user.ID && x.PageID == page.ID); // Kullanıcının o sayfayada yetkisi var mı yok mu onu buluyoruz.
                    //var branchAuth = db.BranchRole.FirstOrDefault(x => x.BrancID == user.BranchID ); //şubesini kontrol ediyoruz.

                    if (auth != null)
                    {
                        switch (Convert.ToInt32(AuthorizationTypes)) // Fonksiyon tarafına gönderilen tiplere göre geriye bir değer dönüyoruz eğer kullanıcının yetkisi varsa true yoksa false olarak dönüş yapıyoruz.
                        {
                            case 1: // Okuma
                                if (auth.IsRead == true)
                                    return true;
                                break;
                            case 2: // Ekleme - Yazma
                                if (auth.IsCreate == true)
                                    return true;
                                break;
                            case 3: // Güncelleme
                                if (auth.IsUpdate == true)
                                    return true;
                                break;
                            case 4: // Silme
                                if (auth.IsDeleted == true)
                                    return true;
                                break;
                        }
                    }
                    return false;

                }
                else
                    return false;
            }
        }

        public static int HolidayPermissionDayCalc(int id)
        {

            using (PortalDB db = new PortalDB())
            {
                try
                {
                    var user = db.SystemUser.FirstOrDefault(x => x.ID == id);
                    var holidayTable = db.HolidayTable.Where(x => x.IsPassive == false).ToList();  // İzin tablosunu çektik buradaki tablodan kullanıcıların yaşlarına ve çalıştıkları günlere göre izin tanımlayabilmek için bir şema oluşturduk

                    var RemainingTime = (DateTime.Now - Convert.ToDateTime(user.JopStartDate)).TotalDays;
                    var years = (DateTime.Now - Convert.ToDateTime(user.BirthDate)).TotalDays / 360;
                    if (years < 18)
                    {

                        return Convert.ToInt32(Convert.ToInt32(holidayTable.FirstOrDefault(x => x.MaxAge == 18).NoDays) - user.PermissionNo);

                    }
                    else
                    {
                        var izinler = db.HolidayTable.Where(x => x.MaxWorkDays > RemainingTime && x.MinWorkDays < RemainingTime).OrderByDescending(x => x.MinWorkDays).First();
                        // Tabloda önce çalışma günümüzün altındaki verileri listeledik ardından elimizdeki listesi
                        // minimum çalışma sayısına en yüksek olanlara göre listeleyip listenin ilk indeksinin izin sayısını aldık

                        return Convert.ToInt32(Convert.ToInt32(izinler.NoDays) - user.PermissionNo);
                    }

                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
    }
}