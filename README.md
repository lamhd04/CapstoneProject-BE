# CapstoneProject-BE
security note:
-password phải được mã hóa, thấp nhất là MD5
-
rules:
-Field phải được ghi cụ thể vd: Object Product có field name (sai) ProductName (đúng)

-Cú pháp tên hàm viết hoa chữ cái đầu của từ vd: GetConfirmationToken

-Parameter phải đặt tên có nghĩa và dễ hiểu vd: GetConfirmationToken(string UserId)

-Tất cả constant phải được ghi trong class Constant.cs với access modifier public và từ khóa static và readonly

-Setting hoặc config sẽ được ghi trong appsetting.json

-Đặt tên controller theo cú pháp A(s) + Controller vd: ProductsController


