using bookpj.DTO;

using bookpj.Entities;

using bookpj.Repository;
using System.Text.Json;



namespace bookpj.Service

{

    public class UserService : IUserService

    {

        private readonly IUserRepository _userRepository;

        private readonly ILogger<UserService> _logger;



        public UserService(IUserRepository userRepository, ILogger<UserService> logger)

        {

            _userRepository = userRepository;

            _logger = logger;

        }



        public async Task<List<UserDTO>> GetAllAsync()

        {

            try

            {

                var users = await _userRepository.GetAllAsync();



                return users.Select(x => new UserDTO

                {

                    UserId = x.UserId,

                    Username = x.Username

                }).ToList();

            }

            catch (Exception ex)

            {

                _logger.LogError(ex, "không có danh sách User");

                throw;

            }

        }



        public async Task<UserDTO?> GetByIdAsync(int id)

        {

            try

            {

                var user = await _userRepository.GetByIdAsync(id);

                if (user == null) return null;



                return new UserDTO

                {

                    UserId = user.UserId,

                    Username = user.Username

                };

            }

            catch (Exception ex)

            {

                _logger.LogError(ex, "không tìm thấy User với ID: {Id}", id);

                throw;

            }

        }



        public async Task<bool> CreateAsync(UserDTO dto)

        {

            var dtoJson = JsonSerializer.Serialize(dto);

            try

            {



                if (string.IsNullOrWhiteSpace(dto.Username))

                    throw new ArgumentException("Tên người dùng không được để trống");



                var user = new User

                {

                    Username = dto.Username

                };



                await _userRepository.AddAsync(user);

                await _userRepository.SaveChangesAsync();



                return true;

            }

            catch (Exception ex)

            {

                _logger.LogError(ex, "Lỗi xảy ra khi tạo người dùng mới: {DtoJson}", dtoJson);

                throw;

            }

        }



        public async Task<bool> UpdateAsync(int id, UserDTO dto)

        {

            var dtoJson = JsonSerializer.Serialize(dto);

            try

            {



                var user = await _userRepository.GetByIdAsync(id);

                if (user == null) return false;



                if (string.IsNullOrWhiteSpace(dto.Username))

                    throw new ArgumentException("Tên người dùng mới không hợp lệ");



                if (user.Username == dto.Username)

                {

                    return true;

                }



                user.Username = dto.Username;

                await _userRepository.SaveChangesAsync();



                return true;

            }

            catch (Exception ex)

            {

                _logger.LogError(ex, "Lỗi xảy ra khi cập nhật người dùng ID: {Id}", id);

                throw;

            }

        }



        public async Task<bool> DeleteAsync(int id)

        {

            try

            {

                var user = await _userRepository.GetByIdAsync(id);

                if (user == null) return false;



                _userRepository.Delete(user);

                await _userRepository.SaveChangesAsync();



                return true;

            }

            catch (Exception ex)

            {

                _logger.LogError(ex, "Lỗi xảy ra khi xóa người dùng ID: {Id}", id);

                throw;

            }

        }

    }

}